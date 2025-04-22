
using Backend.Base.Permission.Ent;

namespace Backend.Base.Permission
{
    public interface PermissionInitialiseServiceI
    {
        void InitialisePermissions();
        List<PermissionEnt> GetPermissions();
    }
}
