using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Test.Core;
using Test.Core.Configuration;

namespace Test.BusinessLogic.Configuration
{
    public static class AutoMapperConfig
    {
        public static void ConfigureServicesAutoMapper(this IServiceCollection services)
        {
            services.ConfigureAutoMapper(new Profile[]
            {
                new MapperProfile()
            });
        }
    }
}

