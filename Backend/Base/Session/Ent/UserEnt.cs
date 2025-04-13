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

        public int LoginId { get; set; }
        public string Userid { get; set; }
        public List<PermissionCrudEnt> Permissions { get; set; }
    }
}
