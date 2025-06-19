using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class OrgDto : _BaseDto<OrgDto>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public string? LangCode { get; set; }
        public int? LangLabelVariant { get; set; }

        public List<OrgLangDto> Languages { get; set; }
        public PasswordRuleDto PasswordRule { get; set; }
    }

    public class OrgLangDto 
    {
        public string? LangCode { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsEditable { get; set; }
    }

    public class PasswordRuleDto
    {
        public int MinLength { get; set; } 
        public int MaxLength { get; set; } 
        public bool IsMixedCase { get; set; }
        public bool IsNumber { get; set; } 
        public bool IsNonLetter { get; set; }
    }

}
