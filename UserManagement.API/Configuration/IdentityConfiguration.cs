using Microsoft.AspNetCore.Identity;
using UserManagement.Infrastructure.Persistence.Context;
using UserManagement.Infrastructure.Persistence.Identity;

namespace UserManagement.API.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
