
/// <summary>
/// Interesction of user - role
/// Created: April 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Role.Ent
{
    public class UserRoleEnt : BaseEntity
    {
        public long RoleId { set; get; }

        public UserRoleEnt() {}

        public override void Decode()
        {

        }
    }
}
