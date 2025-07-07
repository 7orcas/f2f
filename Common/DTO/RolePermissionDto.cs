namespace Common.DTO
{
    public class RolePermissionDto
    {
        public int OrgNr { get; set; }
        public string Role { get; set; }
        public int PermissionNr { get; set; }
        public string LangKey { get; set; }
        public string Crud { get; set; }
    }
}
