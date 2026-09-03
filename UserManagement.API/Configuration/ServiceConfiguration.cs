using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Configuration;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Infrastructure.Repositories;
using UserManagement.Infrastructure.Services;

namespace UserManagement.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IGenericRepository, GenericRepository>();

            services.AddScoped<IBranchService, BranchService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<JwtService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IUserRoleService, UserRoleService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IMenuService, MenuService>();

            services.AddScoped<IRoleMenuService, RoleMenuService>();

            services.AddScoped<IUserProfileService, UserProfileService>();

            return services;
        }
    }
}