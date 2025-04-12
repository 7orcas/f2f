using System.Security;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Session.Ent
{
    public class SessionEnt
    {
        public string Key { get; set; }
        public OrgEnt Org { get; set; }
        public LoginEnt Login { get; set; }

        //ToDo
        public string GetUserPermissionCrud (string perm)
        {
            if (perm.Equals("machine")) return GC.CrudAll;

            return null; //Not found
        }


    }
}
