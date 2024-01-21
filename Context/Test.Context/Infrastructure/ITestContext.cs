using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Context.Infrastructure
{
    public interface ITestContext : IDisposable
    {
        DbSet<Tasks> Tasks { get; set; }

        DbSet<User> User { get; set; }

        int SaveChanges();

    }
}