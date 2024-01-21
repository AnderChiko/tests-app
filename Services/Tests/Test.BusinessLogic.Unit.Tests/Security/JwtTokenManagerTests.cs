using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.BusinessLogic.Authentication;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.BusinessLogic.Models.Authentication;
using Test.Core.Models.Data;

namespace Test.BusinessLogic.Unit.Tests.Security
{
    public class JwtTokenManagerTests
    {

        private readonly Mock<IUserManager> _mockUserManager;
        private readonly JwtTokenManager _jwtTokenManager;
        private readonly Mock<IConfiguration> _mockIConfiguration;
        public JwtTokenManagerTests() {
            _mockUserManager = new Mock<IUserManager>();
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockUserManager.Setup(p => p.Get()).Returns(getInitialDbEntities());

            var _configurationSection = new Mock<IConfigurationSection>();
            _configurationSection.Setup(a => a.Value).Returns("Vtt4d$Q^01!yQF$pYA4B9fZougii8trwp10REp$c$ksWi5");
            _mockIConfiguration.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
            _mockIConfiguration.Setup(a => a.GetSection("TokenProviderOptions:SecretPassword")).Returns(_configurationSection.Object);

            _jwtTokenManager = new JwtTokenManager(_mockIConfiguration.Object, _mockUserManager.Object);
        }

        private Task<DataResult<List<User>>> getInitialDbEntities()
        {
            var userList = new List<User>()
             {
                new User {Id = 1, Username="Test1", Password="Test1"},
                new User {Id = 2, Username="Test2", Password="Test2"},
                new User {Id = 3, Username="Test3", Password="Test3"},
            };

            return Task.Run(() =>
            {
                return new DataResult<List<User>>(userList);
            });
        }

        [Fact]
        public async void Authenticate_ActionExecutes_ReturnsJwtToken()
        {
            var user = new UserCredintial()
            {
                Username = "Test1",
                Password = "Test1"
            };

            var result = await _jwtTokenManager.Authenticate(user.Username,user.Password);

            Assert.NotNull(result);
        }
    }
}
