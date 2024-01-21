using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Test.BusinessLogic.Authentication;
using Test.BusinessLogic.Models.Authentication;
using Test.Core.Models;
using Test.Core.Models.Data;
using Test.Core.Models.Exceptions;
using Test.Core.Models.Security;

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
        public async Task<TokenRespnse> Authenticate([FromBody] UserCredintial userCredintial)
        {
            // add validation model

            var token = await _jwtTokenManager.Authenticate(userCredintial.Username, userCredintial.Password);
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Unauthorized user!");

            return new TokenRespnse()
            {
                Data = new TokenResponeModel()
                {
                    Token = token
                }
            };
        }

    }
}
