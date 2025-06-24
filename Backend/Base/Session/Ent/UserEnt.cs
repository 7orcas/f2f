/**
 * User object for the logged in session
 * 
 * Created 13.04.25 
 * [Licence]
 * @author John Stewart
 */

namespace Backend.Base.Session.Ent
{
    public class UserEnt
    {
        public UserEnt() { }

        public long UserAccountId { get; set; }
        public string Userid { get; set; }
        public List<PermissionCrudEnt> Permissions { get; set; }

        //ToDo move to db
        public bool IsSystemAdmin { get; set; } = true; 
        public bool IsCurrentLanguageAdmin { get; set; } = true;
        public bool IsActiveLanguageAdmin { get; set; } = true;
        public bool IsService { get; set; } = true; 

        public bool IsLanguageAdmin()
        {
            return IsCurrentLanguageAdmin || IsActiveLanguageAdmin || IsService;
        }

    }
}
