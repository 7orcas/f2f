using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class _BaseDto
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
    }
}
