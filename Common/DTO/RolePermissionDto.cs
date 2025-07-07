namespace Common.DTO
{
    public class RolePermissionDto
    {
        public int OrgId { get; set; }
        public string Role { get; set; }
        public string Permission { get; set; }
        public string Crud { get; set; }
    }
}
