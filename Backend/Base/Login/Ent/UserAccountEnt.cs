/// <summary>
/// User login entity
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Login.Ent
{
    public class UserAccountEnt
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int OrgId { get; set; }
        public string? LangCode { get; set; }
        public int? Classification {  get; set; }
        public DateTime Lastlogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
