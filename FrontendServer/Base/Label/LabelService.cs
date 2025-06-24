/// <summary>
/// Scoped label service for the client
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>
namespace FrontendServer.Base.Config
{
    public class LabelService
    {
        private Dictionary<string, LangLabelDto> labels;

        public void SetLabels(Dictionary<string, LangLabelDto> labels) => this.labels = labels;
        public bool IsInitialized => labels != null;

        public bool IsLabel(string labelCode) => labels != null && labels.ContainsKey(labelCode);
        public bool IsTooltip(string labelCode) => IsLabel(labelCode) ? !string.IsNullOrEmpty(labels[labelCode].Tooltip) : false;

        public string GetLabel(string labelCode) => IsLabel(labelCode) ? labels[labelCode].Label : labelCode;
        public string GetLabelHighlightNoKey(string labelCode) => IsLabel(labelCode) ? labels[labelCode].Label : "[" + labelCode + "]";
        public string? GetTooltip(string labelCode) => IsTooltip(labelCode) ? labels[labelCode].Tooltip : null;

    }
}
