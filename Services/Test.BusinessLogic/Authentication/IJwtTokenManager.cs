using System.Threading.Tasks;

namespace Test.BusinessLogic.Authentication
{
    public interface IJwtTokenManager
    {
        Task<string> Authenticate(string username, string password);
    }
}
