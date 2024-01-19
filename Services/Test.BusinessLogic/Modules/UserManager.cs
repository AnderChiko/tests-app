
using AutoMapper;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.Core.Data;

namespace Test.BusinessLogic
{
    public class UserManager : DataManagerBase<Context.User,User, int, Context.TestContext>, IUserManager
    {
        public UserManager(IServiceProvider serviceProvider, IMapper mapper) : base(serviceProvider, mapper)
        {
        }
    }
}