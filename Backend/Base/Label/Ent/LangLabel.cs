
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Base.Label.Ent
{
    public class LangLabel : Encode
    {
        /// <summary>
        /// The label's id.
        /// </summary>
        public int Id { get; set; }

        public int LangKeyId { set; get; }

        /// <summary>
        /// Language code of label.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Organisation number (null is the default).
        /// </summary>
        public int? HardCodedNr { get; set; }

        /// <summary>
        /// Label code.
        /// </summary>
        public string Code { get; set; }
                
        /// <summary>
        /// Label tooltip.
        /// </summary>
        public string? Tooltip { get; set; }

        /// <summary>
        /// Last date/time of update.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Is language code active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Language code of label.
        /// </summary>
        /*
         * Indicates this label has been loaded with this lang code
         */
        [NotMapped]
        public string? LabelCode { get; set; }
    }
}
