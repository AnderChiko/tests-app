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
    public class UserController : CrudControllerBase<UserController, IUserManager, User, int>
    {

        public UserController(IUserManager manager, ILogger<UserController> loggingManager)
         : base(manager, loggingManager)
        {

        }
    }
}
