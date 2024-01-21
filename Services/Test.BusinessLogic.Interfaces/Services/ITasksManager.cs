
using Test.BusinessLogic.Models;
using Test.Core.Interfaces.Data;

namespace Test.BusinessLogic.Interfaces
{
    public interface ITasksManager : ICrudManager<Tasks, int>
    {
    }
}