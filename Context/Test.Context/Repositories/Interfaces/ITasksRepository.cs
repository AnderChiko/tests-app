using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Context;
using Test.Context.Repositories.Interfaces;

namespace Test.Context.Repositories.Interfaces
{
    public interface ITasksRepository: IRepository<Tasks, int>
    {
    }
}
