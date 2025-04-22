
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public interface PermissionServiceI
    {
        PermissionEnt GetPermissionEnt(string perm);
        Task<List<PermissionCrudEnt>> LoadEffectivePermissions(int userId, int orgId);
        Task<List<RolePermissionCrudEnt>> GetPermissions(int userId, int orgId);

        bool IsAuthorizedToAccessEndPoint(SessionEnt session, PermissionAtt perm, CrudAtt crud);
    }
}
