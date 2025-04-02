namespace FrontendLogin.Service
{

    //May not need this

    public class LoginService
    {
        public Task<string> LoginAsync()
        {
            return Task.FromResult("login service");
        }
    }
}
