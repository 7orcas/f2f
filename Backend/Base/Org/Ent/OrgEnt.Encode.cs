using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Encoded part of Organisation entity
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Org.Ent
{
    public partial class OrgEnt
    {

        public override void Decode()
        {
            var o = Decode<OrgEnt>();
            Languages = Get(o.Languages, Languages);
            IsLangCodeEditable = Get(o.IsLangCodeEditable, IsLangCodeEditable);
        }

        /// <summary>
        /// Language codes visible to this org.
        /// </summary>
        [NotMapped]
        public List<string> Languages { get; set; } = new List<string>();

        /// <summary>
        /// Language code labels can be editied.
        /// </summary>
        [NotMapped]
        public bool IsLangCodeEditable { get; set; } = false;

    }
}
