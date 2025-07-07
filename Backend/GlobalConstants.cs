namespace Backend
{
    public class GlobalConstants
    {
        public const int BaseorgNr   = 0;
        public const int NewRecordId = -9;

        public const int AppClient          = 1; //defined in FrontendLogin as well
        public const string LangCodeDefault = "en";

        public const string CrudCreate   = "c";
        public const string CrudRead     = "r";
        public const string CrudUpdate   = "u";
        public const string CrudDelete   = "d";
        public const string CrudReadList = "l";
        public const string CrudIgnore   = "x"; //Don't need a permission crud setting

        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        //IMemoryCache keys
        public const string CacheKeyTokenPrefix     = "TS_";
        public const string CacheKeyOrgPrefix       = "OS_";
        public const string CacheKeyOrgConfigPrefix = "CS_";
        public const string CacheKeySessionPrefix   = "SS_";
        public const string CacheKeyPermDic         = "PS_dic";

        //Entity Type Id
        public const int EntityTypePermission       = 1;
        public const int EntityTypePermissionEffect = 2;
        public const int EntityTypeRole             = 3;
        public const int EntityTypeAudit            = 4;
        public const int EntityTypeLogin            = 5;
        public const int EntityTypeLogout           = 6;
        public const int EntityTypeLangCode         = 7;
        public const int EntityTypeLangKey          = 8;
        public const int EntityTypeLangLabelList    = 9;
        public const int EntityTypeLangLabelRelated = 10;
        public const int EntityTypeConfig           = 11;
        public const int EntityTypeOrg              = 12;

        public const int EntityTypeMachine          = 101;

        public static readonly object[] EntityTypes = {
            EntityTypePermission,       "Permission",
            EntityTypePermissionEffect, "PermissionEffective",
            EntityTypeRole,             "Role",
            EntityTypeAudit,            "Audit",
            EntityTypeLogin,            "Login",
            EntityTypeLogout,           "Logout",
            EntityTypeLangCode,         "LangCode",
            EntityTypeLangKey,          "LangKey",
            EntityTypeLangLabelList,    "LangLabelList",
            EntityTypeLangLabelRelated, "LangLabelRelated",
            EntityTypeConfig,           "Config",
            EntityTypeOrg,              "Org",

            EntityTypeMachine,          "Machine",
        };

        //Service
        public const long ServiceLoginId             = -1L;
        public const long ServiceAccountId           = -1L;
        public const string ServiceAccountName       = "Service";

    }
}
