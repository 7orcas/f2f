
using Backend.Base.Role.Ent;

namespace Backend.Base.Role
{
    public interface RoleServiceI
    {
        Task<List<UserAccountRoleEnt>> GetUserRoles(SessionEnt session);

    }
}
