using GC = Backend.GlobalConstants;

/// <summary>
/// User login entity
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Login.Ent
{
    public class UserAccountEnt : BaseEncode
    {
        public long Id { get; set; }
        public long LoginId { get; set; }
        public int OrgId { get; set; }
        public string? LangCode { get; set; }
        public int? Classification {  get; set; }
        public DateTime Lastlogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        //Update note: Add to service

        public override void Decode() { }
        public override void Encode() { }

        public bool IsService() => LoginId == GC.ServiceLoginId;

        /*
         * Special service account
         * Account does not have to be in the database
         */
        public static UserAccountEnt GetServiceAccount(int orgId)
        {
            return new UserAccountEnt
            {
                Id = GC.ServiceAccountId,
                LoginId = GC.ServiceLoginId,
                OrgId = orgId,
                LangCode = GC.LangCodeDefault,
                Lastlogin = DateTime.Now,
                IsActive = true,
                IsAdmin = true
            };
        }
    }
}
