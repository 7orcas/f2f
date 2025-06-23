namespace FrontendServer.Base._Base
{
    public partial class BasePage
    {
        protected async Task SetLabels()
        {
            if (_config == null) return;
            _labels = CacheService.GetLabels(_config.Label.LangCode);
        }

        public bool IsLabel(string labelCode) => _labels != null && _labels.ContainsKey(labelCode);
        public bool IsTooltip(string labelCode) => IsLabel(labelCode) ? !string.IsNullOrEmpty(_labels[labelCode].Tooltip) : false;

        public string GetLabel(string labelCode) => IsLabel(labelCode)? _labels[labelCode].Label : labelCode;
        public string GetLabelHighlightNoKey(string labelCode) => IsLabel(labelCode) ? _labels[labelCode].Label : "[" + labelCode + "]";
        public string? GetTooltip(string labelCode) => IsTooltip(labelCode) ? _labels[labelCode].Tooltip : null;
        
    }
}
