
/// <summary>
/// Configurations are for organisation
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Config.Ent
{
    public class OrgConfig
    {
        public int OrgId { get; set; }
        public string LangCodeDefault { get; set; }
        public List<LanguageConfig> Languages { get; set; }
    }
}
