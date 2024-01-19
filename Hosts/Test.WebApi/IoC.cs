using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Core;
using Test.Context;
using Test.BusinessLogic;
using Test.WebApi.PolicyHandlers;

namespace Test.WebApi
{
    public static class IoC
    {
        /// <summary>
        /// Method to register the Core dependencies.
        /// 
        /// Transient: A new instance of the type is used every time the type is requested.
        /// 
        /// Scoped: A new instance of the type is created the first time it’s requested within
        ///			a given HTTP request, and then re - used for all subsequent types resolved
        ///			during that HTTP request.
        ///			
        /// Singleton: A single instance of the type is created once, and used by all subsequent
        ///			requests for that type.
        ///			
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContextIoC();
            services.AddCoreServices();
            services.AddBusinessLogicServices();

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContextOptions(configuration);
            services.AddConfigurationOptions(configuration);

            return services;
        }

        /// <summary>
        /// Configure Cors
        /// See: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("AllowedOrigins").Value.Split(',');

            string[] corsHeaders =
            {
                ApplicationHeaders.ApiVersion,
                "content-disposition",
                "content-length",
                "content-type"
            };

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins(origins)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithExposedHeaders(corsHeaders)
                        .AllowAnyMethod());
            });

            return services;
        }
    }
}
