namespace FrontendServer.Base.Util
{
    public class ToolBarButton
    {
        public string LangKey { get; set; }
        public string Class { get; set; } = "toolbar-button";
        public Func<Task> Action { get; set; }

    }
}
