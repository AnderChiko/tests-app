using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Test.Core.Extensions;
using Test.WebApi.PolicyHandlers;
using Test.BusinessLogic.Configuration;

namespace Test.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
            services.ConfigureCorsServices(Configuration);
            services.AddHttpClient();
            services.AddOptions();
            services.AddOptions(Configuration);

            services.ConfigureServicesAutoMapper();

            services.AddHttpContextAccessor();
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // https://stackoverflow.com/questions/2441290/javascriptserializer-json-serialization-of-enum-as-string/2870420#2870420
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "test API", Version = "v1" });
                c.EnableAnnotations();

                // Build operation Id based on old v3 of SwashBuckle
                c.CustomOperationIds(apiDescription => apiDescription.FriendlyId());

                //To Enable authorization using Swagger (JWT)  
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                In = ParameterLocation.Header,
                            },
                            new string[] {}

                    }
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;

                var key = Configuration.GetValue<string>("TokenProviderOptions:SecretPassword");
                var keyBytes = Encoding.ASCII.GetBytes(key);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["TokenProviderOptions:Issuer"],
                    ValidAudience = Configuration["TokenProviderOptions:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();

            app.UseAuthorization();

            var apiVersionHeader = Configuration.GetSection(ApplicationHeaders.ConfigKey(ApplicationHeaders.ApiVersion)).Value;

            app.Use((context, next) =>
            {
                context.Response.Headers[ApplicationHeaders.ApiVersion] = apiVersionHeader;
                return next.Invoke();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
