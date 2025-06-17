
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public interface PermissionServiceI
    {
        PermissionEnt GetPermissionEnt(string perm);
        Task<List<PermissionCrudEnt>> LoadEffectivePermissions(SessionEnt session);
        Task<List<PermissionCrudEnt>> LoadEffectivePermissionsInt(long userId, long orgId);
        Task<List<RolePermissionCrudEnt>> GetPermissions(SessionEnt session);

        bool IsAuthorizedToAccessEndPoint(SessionEnt session, PermissionAtt perm, CrudAtt crud);
    }
}
