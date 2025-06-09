namespace FrontendServer.Base
{
    public partial class BasePage
    {
        public async void SetLabels()
        {
            if (_config == null)
                await SetConfig();
            _labels = Cache.GetLabels(_config.LangCode);
        }

        public string GetLabel(string labelCode)
        {
            SetLabels();
            if (_labels != null && _labels.ContainsKey(labelCode))
                return _labels[labelCode].Label;
            return "?" + labelCode;
        }

        public string? GetTooltip(string labelCode)
        {
            SetLabels();
            if (_labels != null && _labels.ContainsKey(labelCode))
                return _labels[labelCode].Tooltip;
            return null;
        }

        public bool IsLabel(string labelCode)
        {
            SetLabels();
            return _labels != null && _labels.ContainsKey(labelCode);
        }

        public bool IsTooltip(string labelCode)
        {
            SetLabels();
            if (_labels != null && _labels.ContainsKey(labelCode))
                return !string.IsNullOrEmpty(_labels[labelCode].Tooltip);
            return false;
        }
    }
}
