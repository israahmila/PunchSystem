using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.DTOs;
using PunchSystem.Models;

namespace PunchSystem.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var username = request.Username.Trim().ToLower();
        var exists = await _context.Users.AnyAsync(u => u.Username == username);
        if (exists) return false;

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
        if (role == null) throw new Exception($"Role '{request.Role}' does not exist.");

        var user = new User
        {
            Id = 0, // Auto-incremented by the database
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = role.Id,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, string ipAddress)
    {
        var username = request.Username.Trim().ToLower();

        var user = await _context.Users
            .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
            .Include(u => u.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is inactive or blocked.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 3)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
                throw new UnauthorizedAccessException("Account blocked after 3 failed login attempts.");
            }

            await _context.SaveChangesAsync();
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Reset failed attempts
        user.FailedLoginAttempts = 0;

        // Save login history
        _context.LoginHistories.Add(new LoginHistory
        {
            UserId = user.Id,
            LoginTime = DateTime.UtcNow,
            IPAddress = ipAddress
        });

        await _context.SaveChangesAsync();

        // Calculate effective permissions
        var userPermissions = user.UserPermissions.Select(up => up.Permission.Name);
        var rolePermissions = user.Role.RolePermissions.Select(rp => rp.Permission.Name);

        var effectivePermissions = userPermissions
            .Union(rolePermissions)
            .Distinct()
            .ToList();

        var token = GenerateJwtToken(user, effectivePermissions);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role.Name,
            Permissions = effectivePermissions
        };
    }

    private string GenerateJwtToken(User user, List<string> effectivePermissions)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.Name)
        };

        claims.AddRange(effectivePermissions.Select(p => new Claim("Permission", p)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
