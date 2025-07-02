namespace FrontendServer.Base._Base
{
    public class BasePageRoutes
    {
        //Routes
        public const string LogoutRoute    = "logout";
        public const string ChangePWRoute  = "changePW";
        public const string OrgRoute       = "organisation";
        public const string ConfigRoute    = "config";
        public const string PermRoute      = "permission";
        public const string RolesRoute     = "role";
        public const string AuditRoute     = "audit";

        //Page Codes
        public const string ChangePWCode   = "cpw001";
        public const string OrgPageCode    = "org001";
        public const string ConfigPageCode = "con001";
        public const string PermPageCode   = "per001";
        public const string RolesPageCode  = "rol001";
        public const string AuditPageCode  = "aud001";

        static protected readonly string[] BasePageCodeRoutes = {
            ChangePWCode,           ChangePWRoute,
            OrgPageCode,            OrgRoute,
            ConfigPageCode,         ConfigRoute,
            PermPageCode,           PermRoute,
            RolesPageCode,          RolesRoute,
            AuditPageCode,          AuditRoute,
        };

    }
}
