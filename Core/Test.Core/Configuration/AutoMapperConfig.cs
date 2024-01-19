using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services, IEnumerable<Profile> enumerableOfProfiles)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = enumerableOfProfiles ?? throw new ArgumentNullException(nameof(enumerableOfProfiles));

            services.AddAutoMapper(options =>
            {
                options.AddExpressionMapping();
                options.AddProfiles(enumerableOfProfiles);
            });
        }
    }
}
