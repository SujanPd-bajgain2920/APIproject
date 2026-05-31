using APIproject.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace APIproject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // MediatR — scans all handlers inside Application layer
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(RegisterUserCommand).Assembly));

            return services;
        }
    }
}