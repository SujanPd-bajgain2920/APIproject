using APIproject.Domain.Interfaces;
using APIproject.Infrastructure.Configuration;
using APIproject.Infrastructure.Persistence;
using APIproject.Infrastructure.Repositories;
using APIproject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace APIproject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // register DbContext with SQL Server provider
            services.AddDbContext<FypContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Sujan")));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // register services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();

            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));


            // HttpContext for session management in handlers
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
