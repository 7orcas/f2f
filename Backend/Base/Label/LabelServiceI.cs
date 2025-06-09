using Backend.Base.Label.Ent;

namespace Backend.Base.Label
{
    public interface LabelServiceI
    {
        Task<List<LangLabel>> GetLanguageLabelList(string langKeyCode);
        Task<LangLabel> GetLanguageLabel(int id);
        Task<List<LangLabel>> GetAllLanguageLabels();
        Task<bool> SaveLabel(LangLabel label);
    }
}
