using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AppConfigDto
    {
        public bool IsLabelLink { get; set; }
        public int OrgId { get; set; }
        public string OrgDescription { get; set; }
        public string LangCode { get; set; }
        public int UniqueUserId { get; set; }
        public int UniqueSessionId { get; set; }

        public LabelConfigDto Label {  get; set; }
    }

    public class LabelConfigDto 
    { 
        public bool ShowLink  { get; set; } = false;
        public bool ShowNoKey  { get; set; } = false;
        public bool ShowModal  { get; set; } = false;
        public bool ShowTooltip  { get; set; } = false;
    }

}
