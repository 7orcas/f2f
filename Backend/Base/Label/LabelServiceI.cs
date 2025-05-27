using Backend.Base.Label.Ent;

namespace Backend.Base.Label
{
    public interface LabelServiceI
    {
        Task<List<LangLabel>> GetLabels(string langCode);
        Task<bool> SaveLabel(LangLabel label);
    }
}
