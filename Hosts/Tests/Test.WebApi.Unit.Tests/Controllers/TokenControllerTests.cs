using Moq;
using System.Linq.Expressions;
using Test.BusinessLogic.Authentication;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models.Authentication;
using Test.Core.Models.Security;
using Test.WebApi.Controllers;

namespace Test.WebApi.Unit.Tests.Controllers
{
    public class TokenControllerTests
    {
        private readonly Mock<IJwtTokenManager> _mockJwtTokenManager;
        private readonly Mock<IUserManager> _mockUserManager;
        private readonly TokenController _controller;
        public TokenControllerTests() {

            _mockJwtTokenManager = new Mock<IJwtTokenManager>();

            _mockJwtTokenManager.Setup(x=> x.Authenticate("username", "password")).Returns(mockResult());
            _controller = new TokenController(_mockJwtTokenManager.Object);
        }

        private async Task<string> mockResult()
        {
            
          var tokenRespnse =  await Task.Run(()=>
            {
                var result = new TokenRespnse()
                {
                    Data = new TokenResponeModel()
                    {
                        Token = "asasassssaxcedcdececed3ee3dfd3fdd3d3d3d3d3"
                    }
                };
                return result;
            });
            return tokenRespnse?.Data?.Token;
        }

        [Fact]
        public async void Authenticate_ActionExecutes_ReturnsJwtToken()
        {
            var user = new UserCredintial()
            {
                Username = "username",
                Password = "password"
            };


            var result = await  _controller.Authenticate(user);

            Assert.NotNull(result);
            Assert.Equal("asasassssaxcedcdececed3ee3dfd3fdd3d3d3d3d3", result.Data?.Token);

        }
    }
}
