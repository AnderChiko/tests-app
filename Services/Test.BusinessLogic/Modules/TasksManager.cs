
using AutoMapper;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.Core.Data;

namespace Test.BusinessLogic
{
    public class TasksManager : DataManagerBase<Context.Tasks,Tasks, int, Context.TestContext>, ITasksManager
    {
        public TasksManager(IServiceProvider serviceProvider, IMapper mapper) : base(serviceProvider, mapper)
        {
        }
    }
}