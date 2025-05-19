using Microsoft.AspNetCore.Identity.Data;
using PunchSystem.DTOs;
using LoginRequest = PunchSystem.DTOs.LoginRequest;
using RegisterRequest = PunchSystem.DTOs.RegisterRequest;

namespace PunchSystem.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request, string ipAddress);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
