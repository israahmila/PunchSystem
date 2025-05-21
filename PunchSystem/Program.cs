using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PunchSystem.Contracts;
using PunchSystem.Data;
using PunchSystem.Security;
using PunchSystem.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔐 CONFIG JWT AUTHENTICATION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 🛡️ CONFIG PERMISSION-BASED AUTHORIZATION
builder.Services.AddAuthorization(options =>
{
    var permissions = new[]
    {
        PermissionPolicies.ManageUsers,
        PermissionPolicies.ManageRoles,
        PermissionPolicies.ManageProduits,
        PermissionPolicies.ManagePoincons,
        PermissionPolicies.ManageFournisseurs,
        PermissionPolicies.ManageMarques,
        PermissionPolicies.CreateUtilisation,
        PermissionPolicies.ViewStats,
        PermissionPolicies.ViewAudit
    };

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});
// 🌐 CORS CONFIG (Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// 🧠 DEPENDENCY INJECTION

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<IPoinconService, PoinconService>();
builder.Services.AddScoped<IFournisseurService, FournisseurService>();
builder.Services.AddScoped<IMarqueService, MarqueService>();
builder.Services.AddScoped<IUtilisationService, UtilisationService>();
builder.Services.AddScoped<IEntretienService, EntretienService>();
builder.Services.AddScoped<IAuditTrailService, AuditTrailService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 📦 CONTROLLERS
builder.Services.AddControllers();

// 📚 SWAGGER CONFIG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PunchSystem API",
        Version = "v1"
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // ✅ Logs dans la console


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    DbInitializer.SeedRoles(db);
//}

// 🚀 MIDDLEWARE PIPELINE
app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
