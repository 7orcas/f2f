namespace Backend.App.Login.Ent
{
    public class Login
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Orgs { get; set; }
        public int Attempts { get; set; }
        public DateTime Lastlogin { get; set; }
        public bool IsActive { get; set; }
    }
}
