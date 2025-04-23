
using Backend.Base.Role.Ent;

namespace Backend.Base.Role
{
    public interface RoleServiceI
    {
        Task<List<UserRoleEnt>> GetUserRoles(int userId, int orgId);

    }
}
