using test_api.Authentication;
using test_api.Interfaces;

namespace test_api.Services
{
    public class UserService : IUserService
    {
        public async Task<User> Authenticate(string userName, string password)
        {
            User user = new();

            if (userName == "Test" && password == "Test")
                user = new User { Id = 1, UserName = userName, Password = password };
            else
                user = null;

            return await Task.FromResult(user);
        }
    }
}
