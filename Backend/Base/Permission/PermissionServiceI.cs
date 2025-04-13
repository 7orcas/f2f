
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public interface PermissionServiceI
    {
        PermissionEnt GetPermissionEnt(string perm);
        Task<List<PermissionCrudEnt>> LoadPermissions(int userId, int orgId);
        bool IsAuthorizedCall(SessionEnt session, PermissionAtt perm, CrudAtt crud);
    }
}
