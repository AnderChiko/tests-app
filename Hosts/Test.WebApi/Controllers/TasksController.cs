using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.Core.Web;

namespace Test.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : CrudControllerBase<TasksController, ITasksManager,Tasks, int>
    {

        public TasksController(ITasksManager manager, ILogger<TasksController> loggingManager)
         : base(manager, loggingManager)
        {

        }
    }
}