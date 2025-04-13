namespace Backend.Base.Permission.Ent
{
    /// <summary>
    /// Hardcoded Permission 
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 13, 2025</created>
    /// <license>**Licence**</license>
    public class PermissionEnt
    {
        public PermissionEnt() { }
        public int PermissionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
