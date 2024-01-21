using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Context.Models;

namespace Test.Context.Infrastructure
{
    public class DbContextGenerator : IDbContextGenerator
    {
        private readonly IOptions<AppSettings> options;
        public DbContextGenerator(IOptions<AppSettings> options)
        {
            this.options = options;
        }

        public ITestContext GenerateMyDbContext()
        {
            return new TestContext(options.Value.ConnString);
        }
    }
}