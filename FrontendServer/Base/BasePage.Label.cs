namespace FrontendServer.Base
{
    public partial class BasePage
    {
        protected async Task SetLabels()
        {
            if (_config == null) return;
            _labels = Cache.GetLabels(_config.Label.LangCode);
        }

        public string GetLabel(string labelCode)
        {
            if (_labels != null && _labels.ContainsKey(labelCode))
                return _labels[labelCode].Label;
            return "?" + labelCode;
        }

        public string GetLabelNoHighlight(string labelCode)
        {
            if (_labels != null && _labels.ContainsKey(labelCode))
                return _labels[labelCode].Label;
            return labelCode;
        }

        public string? GetTooltip(string labelCode)
        {
            if (_labels != null && _labels.ContainsKey(labelCode))
                return _labels[labelCode].Tooltip;
            return null;
        }

        public bool IsLabel(string labelCode)
        {
            return _labels != null && _labels.ContainsKey(labelCode);
        }

        public bool IsTooltip(string labelCode)
        {
            if (_labels != null && _labels.ContainsKey(labelCode))
                return !string.IsNullOrEmpty(_labels[labelCode].Tooltip);
            return false;
        }
    }
}
