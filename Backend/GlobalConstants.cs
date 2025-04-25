namespace Backend
{
    public class GlobalConstants
    {
        public const int BaseOrgId = 0;

        public const string CrudCreate   = "c";
        public const string CrudRead     = "r";
        public const string CrudUpdate   = "u";
        public const string CrudDelete   = "d";
        public const string CrudReadList = "l";

        public const int AppClient        = 1; //defined in FrontendLogin as well


        public const int EntityPermission = 1;
        public const int EntityRole       = 2;
        public const int EntityAudit      = 3;
        public const int EntityMachine    = 101;

        public static readonly object[] Entities = {
            EntityPermission, "Permission",
            EntityRole,       "Role",
            EntityAudit,      "Audit",

            EntityMachine,    "Machine",
        };

    }
}
