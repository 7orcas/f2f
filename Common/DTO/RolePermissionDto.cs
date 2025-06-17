namespace Common.DTO
{
    public class RolePermissionDto
    {
        public string Role { get; set; }
        public long OrgId { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionDescr { get; set; }
        public string Crud { get; set; }

    }
}
