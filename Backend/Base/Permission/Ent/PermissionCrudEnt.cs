namespace Backend.Base.Permission.Ent
{
    /// <summary>
    /// Resolved login / permission / crud for the logged in org
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 13, 2025</created>
    /// <license>**Licence**</license>
    public class PermissionCrudEnt
    {
        public PermissionCrudEnt() { }
        public int PermissionId {  get; set; }
        public string Crud { get; set; }
    }
}
