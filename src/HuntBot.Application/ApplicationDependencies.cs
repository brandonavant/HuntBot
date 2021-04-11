using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HuntBot.Application
{
    public static class ApplicationDependencies
    {
        /// <summary>
        /// Adds dependencies to the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance to which dependencies are added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> containing dependencies specifically for the Application project.</returns>
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}