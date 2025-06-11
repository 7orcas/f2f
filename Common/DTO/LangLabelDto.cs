using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class LangLabelDto
    {
        public int Id { get; set; }
        public int LangKeyId { set; get; }
        public string LangKeyCode { get; set; }
        public int? HardCodedNr { get; set; }
        public string LangCode { get; set; }
        public string Label { get; set; }
        public string? Tooltip { get; set; }
        public DateTime Updated { get; set; }
                
        public bool IsUpdateable { get; set; } = false;
    }
}
