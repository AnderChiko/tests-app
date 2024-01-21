using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Context;

namespace Test.BusinessLogic.Interfaces.Repositories
{
    public interface ITasksRepository: IRepository<Tasks, int>
    {
    }
}
