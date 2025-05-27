using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Language Key
 */
namespace Backend.Base.Label.Ent
{
    public class LangKey : Encode
    {
        /// <summary>
        /// The language code's id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The language key's packs
        /// </summary>
        public string Pack { get; set; }

        /// <summary>
        /// The language code's id.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The language code's description.
        /// </summary>
        public string? Description { get; set; }

        public DateTime Updated { get; set; }

        /// <summary>
        /// Is language code active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
