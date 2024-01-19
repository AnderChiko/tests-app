using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Test.Context;
using Test.BusinessLogic;
using Test.Core.Configuration;

namespace Test.Core.Testing
{
    public abstract class TestHarnessBase
    {
        protected CancellationTokenSource _cancellationTokenSource;
        protected CancellationToken _cancellationToken;

        protected IServiceCollection _services;
        protected IServiceProvider _serviceProvider;
        protected IServiceScopeFactory _servicescopeFactory;

        protected IConfiguration _configuration;
        
        protected Mock<ILogger> _mockedLogger = new Mock<ILogger>();
        protected Dictionary<string, string> _currentRequestProperties = new Dictionary<string, string>();

        public TestHarnessBase()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = _cancellationTokenSource.Token;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            _configuration = configBuilder.Build();

            this._services = new ServiceCollection();

            _services.AddSingleton<IConfiguration>((serviceProvider) => _configuration);

            _services.AddContextIoC();
            _services.AddCoreServices();
            _services.AddBusinessLogicServices();

            _services.AddOptions();
            _services.AddContextOptions(_configuration);
            _services.AddContextOptions(_configuration);
           
            ReBuildServices();
        }

        protected void ReBuildServices()
        {
            this._serviceProvider = this._services.BuildServiceProvider();
            this._servicescopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
        }

        protected TResult RunInScope<TInterface, TResult>(Func<TInterface, IServiceScope, TResult> func)
        {
            using (var scope = _servicescopeFactory.CreateScope())
            {
                var executionClass = scope.ServiceProvider.GetRequiredService<TInterface>();

                return func(executionClass, scope);
            }
        }

    }
}
