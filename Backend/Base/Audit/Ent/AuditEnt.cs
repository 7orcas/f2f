using System;

namespace Backend.Base.Audit.Ent
{
    public class AuditEnt
    {
        public int Id {  get; set; }
        public int OrgId { get; set; }
        public int Source {  get; set; }
        public int Entity {  get; set; }
        public int? EntityId { get; set; }
        public int UserId { get; set; }
        public DateTime Created {  get; set; }
	    public string Crud {  get; set; }
        public string? Details { get; set; }

    }
}
