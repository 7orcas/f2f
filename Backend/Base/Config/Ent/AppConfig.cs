
namespace Backend.Base.Config.Ent
{
    public class AppConfig
    {
        public int OrgId { get; set; }
        public string LangCode { get; set; }
        public List<LanguageConfig> Languages { get; set; }
    }

    public class LanguageConfig
    {
        public string LangCode { get; set; }
        public bool IsCreateable { get; set; } = false;
        public bool IsReadable { get; set; } = false;
        public bool IsUpdateable { get; set; } = false;
    }
}
