namespace FrontendServer.Base._Base
{
    public class BasePageRoutes
    {
        //Routes
        public const string OrgRoute       = "organisation";
        public const string ConfigRoute    = "config";

        //Page Codes
        public const string OrgPageCode    = "org001";
        public const string ConfigPageCode = "con001";


        static protected readonly string[] BasePageCodeRoutes = {
            OrgPageCode,            OrgRoute,
            ConfigPageCode,         ConfigRoute,

        };

    }
}
