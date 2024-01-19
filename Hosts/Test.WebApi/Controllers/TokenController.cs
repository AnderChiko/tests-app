using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Test.BusinessLogic.Authentication;
using Test.BusinessLogic.Models.Authentication;

namespace Test.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        public TokenController(IJwtTokenManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserCredintial userCredintial)
        {

            var token = await _jwtTokenManager.Authenticate(userCredintial.Username, userCredintial.Password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(token);
        }

    }
}
