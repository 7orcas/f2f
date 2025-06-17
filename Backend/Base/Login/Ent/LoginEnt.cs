namespace Backend.Base.Login.Ent
{
    public class LoginEnt
    {
        public long Id { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Orgs { get; set; }
        public string? LangCode { get; set; }
        public int? Attempts { get; set; }
        public DateTime Lastlogin { get; set; }
        public bool IsActive { get; set; }

        //Login Repsonse variables
        public LoginResponse Response { get; set; } = new LoginResponse();
    }

    public class LoginResponse
    {
        public bool Valid { get; set; } = false;

        public string TokenKey { get; set; }
        public string Token { get; set; }
        public string MainUrl { get; set; }
        public string LangCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
