namespace Backend.Base.Permission.Ent
{
    /// <summary>
    /// Role / permission / crud for the logged in user / org
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 13, 2025</created>
    /// <license>**Licence**</license>
    public class RolePermissionCrudEnt
    {
        public RolePermissionCrudEnt() { }
        public string Role {  get; set; }
        public string PermissionCode { get; set; }
        public string PermissionsDescr { get; set; }
        public string Crud { get; set; }
        public long OrgId { get; set; }
    }
}
