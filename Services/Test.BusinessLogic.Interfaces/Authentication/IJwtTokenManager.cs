using System.Threading.Tasks;

namespace Test.BusinessLogic.Interfaces.Authentication
{
    public interface IJwtTokenManager
    {
        Task<string> Authenticate(string username, string password);
    }
}