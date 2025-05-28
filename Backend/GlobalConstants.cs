namespace Backend
{
    public class GlobalConstants
    {
        public const int BaseOrgId = 0;

        public const int AppClient          = 1; //defined in FrontendLogin as well
        public const string LangCodeDefault = "en";

        public const string CrudCreate   = "c";
        public const string CrudRead     = "r";
        public const string CrudUpdate   = "u";
        public const string CrudDelete   = "d";
        public const string CrudReadList = "l";
        public const string CrudIgnore   = "x"; //Don't need a permission crud setting

        //Entity Type Id
        public const int EntityTypePermission       = 1;
        public const int EntityTypePermissionEffect = 2;
        public const int EntityTypeRole             = 3;
        public const int EntityTypeAudit            = 4;
        public const int EntityTypeLogin            = 5;
        public const int EntityTypeLogout           = 6;
        public const int EntityTypeLangCode         = 7;
        public const int EntityTypeLangKey          = 8;
        public const int EntityTypeLangLabel        = 9;
        public const int EntityTypeConfig           = 11;

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
            EntityTypeLangLabel,        "LangLabel",
            EntityTypeConfig,           "Config",

            EntityTypeMachine,          "Machine",
        };

    }
}
