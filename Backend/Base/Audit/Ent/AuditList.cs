namespace Backend.Base.Audit.Ent
{
    public class AuditList : AuditEnt
    {
        public string EntityName { get; set; }
        public string User { get; set; }
        public string CrudDescr { get; set; }
        public string SourceDescr { get; set; }
    }
}
