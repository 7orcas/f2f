
/// <summary>
/// Common constants between all projects
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Common
{
    public class GlobalConstants
    {
        public const int StatusCodeNotAuthorised = 403;

        //Field Lengths
        public const int LenCode        = 15;
        public const int LenDescription = 50;
        public const int LenLangCode    = 4;


        //Permissions
        public const int PerOrg     = 1;
        public const int PerPerm    = 2;
        public const int PerLabel   = 3;
        public const int PerLang    = 4;
        public const int PerRole    = 5;
        public const int PerUser    = 6;
        public const int PerAudit   = 7;
        public const int PerConfig  = 8;

        public const int PerMach    = 101;

        public static readonly object[] Permissions = {
            PerOrg,      "Org",
            PerPerm,     "Perm", 
            PerLabel,    "Label",
            PerLang,     "Lang",
            PerRole,     "Role",
            PerUser,     "User",
            PerAudit,    "Audit",
            PerConfig,   "Conf",

            PerMach,     "Machine"
        };

    }
}
