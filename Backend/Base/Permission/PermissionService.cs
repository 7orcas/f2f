
using Azure.Core;
using GC = Backend.GlobalConstants;
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public class PermissionService: BaseService, PermissionServiceI
    {
        public bool IsAuthorizedCall(SessionEnt session, PermissionAtt perm, CrudAtt crud)
        {
            if (perm == null) return true;
            if (session == null) return false;

            var userCrud = session.GetUserPermissionCrud(perm.Name);
            if (userCrud == null) return false; //permission not found in user profile
            if (crud == null || userCrud.Equals(GC.CrudAll)) return true;

            return userCrud.IndexOf(crud.Action) != -1;
        }
    }
}
