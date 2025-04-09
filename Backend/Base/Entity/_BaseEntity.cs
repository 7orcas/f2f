namespace Backend.Base.Entity
{
    public class _BaseEntity : _Encode
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }

        public _BaseEntity() { }
    }
}
