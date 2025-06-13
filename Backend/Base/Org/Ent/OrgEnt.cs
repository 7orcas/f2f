using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Organisation entity
/// Required for each customer using this application
/// Created: March 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Org.Ent
{
    public partial class OrgEnt : Encode
    {
        public int Id { get; set; }
        public int Nr { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public string? LangCode { get; set; }
        public int? LangLabelVariant { get; set; }
    }
}
