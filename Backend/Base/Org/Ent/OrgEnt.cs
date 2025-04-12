namespace Backend.Base.Org.Ent
{
    public class OrgEnt : Encode
    {
        public int Id { get; set; }
        public int Nr { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
    }
}
