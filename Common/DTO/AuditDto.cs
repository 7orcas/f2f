using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AuditDto
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int Source { get; set; }
        public string EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? User { get; set; }
        public DateTime Created { get; set; }
        public string Crud { get; set; }
        public string? Details { get; set; }
    }
}
