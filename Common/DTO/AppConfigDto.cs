
namespace Common.DTO
{
    public class AppConfigDto : _BaseDto<AppConfigDto>
    {
        public long OrgId { get; set; }
        public string OrgDescription { get; set; }
        public long UniqueUserId { get; set; }
        public long UniqueSessionId { get; set; }
        public LanguageConfigDto[] Languages { get; set; }
        public LabelConfigDto Label {  get; set; }
    }

    public class LabelConfigDto 
    { 
        //Current language code
        public string LangCode { get; set; }
        public bool ShowTooltip  { get; set; } = false;
        public bool IsAdminLanguage { get; set; } = false;
        public bool HighlightNoKey  { get; set; } = false;
    }

    public class LanguageConfigDto
    {
        public string LangCode { get; set; }
        public bool IsUpdateable { get; set; } = false;
    }

}
