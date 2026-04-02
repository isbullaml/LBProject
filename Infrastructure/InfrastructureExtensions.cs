using Core.Interfaces;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SqlService>();
            services.AddTransient<HttpClient>();
            services.AddScoped<IBreachRepository, BreachRepository>();
            services.AddScoped<IPwnApiService, PwnApiService>();

            return services;
        }
    }
}
