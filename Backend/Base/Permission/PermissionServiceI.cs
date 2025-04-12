
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public interface PermissionServiceI
    {
        bool IsAuthorizedCall(SessionEnt session, PermissionAtt perm, CrudAtt crud);
    }
}
