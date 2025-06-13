namespace Backend.Base.Login.Ent
{
    public class LoginEnt
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Orgs { get; set; }
        public string? LangCode { get; set; }
        public int? Attempts { get; set; }
        public DateTime Lastlogin { get; set; }
        public bool IsActive { get; set; }
    }
}
