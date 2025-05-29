namespace FrontendServer
{
    public class GlobalConstants
    {
        public const int AppClient                = 1; //defined in FrontendLogin as well
        
        public const string LabelCacheKey         = "kLabel";
        public const string TokenCacheKey         = "kToken";
        public const string LoggedInCacheKey      = "kLoggedIn";
        public const string ConfigCacheKey        = "kConfig";
        public const string AuthorizedClientKey   = "kAC";
        public const string UnAuthorizedClientKey = "kUAC";
        public const string BearerKey             = "Bearer";


        public const string URL_logout            = "api/Logout/logout";
        public const string URL_perm_list         = "api/Permission/list";
        public const string URL_perm_eff          = "api/Permission/listeffective";
        public const string URL_audit_list        = "api/Audit/list";
        public const string URL_role_list         = "api/Role/list";
        public const string URL_config            = "api/Config/get";
        public const string URL_label_list        = "api/Label/list/";
    }
}
