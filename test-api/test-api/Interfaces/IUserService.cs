using test_api.Authentication;

namespace test_api.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string userName, string password);
    }
}
