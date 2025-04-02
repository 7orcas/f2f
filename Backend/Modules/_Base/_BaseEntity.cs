
namespace Backend.Modules._Base
{
    public class _BaseEntity : _Encode
    {
        public int Id { get; set; }
        public int _OrgId { get; set; }
        public string Descrim { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }

        public _BaseEntity() { }
    }
}
