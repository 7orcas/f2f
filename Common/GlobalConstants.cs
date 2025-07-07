
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
        public const string PermOrg     = "org";
        public const string PermPerm    = "perm";
        public const string PermLabel   = "label";
        public const string PermLang    = "lang";
        public const string PermRole    = "role";
        public const string PermUser    = "user";
        public const string PermAudit   = "audit";

        public const string PermMach    = "machine";

        public static readonly string[] Perms = {
            PermOrg,
            PermPerm,
            PermLabel,
            PermLang,
            PermRole,
            PermUser,
            PermAudit,

            PermMach
        };

    }
}
