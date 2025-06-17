using GC = Backend.GlobalConstants;
using Microsoft.Data.SqlClient;

/// <summary>
/// Utility class to load org entities.
/// Used by singleton service and other services (keeps the DRY princple)
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Org
{
    public class OrgLoad : SqlUtils
    {
        static public OrgEnt Load(SqlDataReader r)
        {
            var org = new OrgEnt();
            org.Id = GetId(r);
            org.Nr = GetNr(r);
            org.Code = GetCode(r);
            org.Description = GetDescription(r);
            org.Updated = GetUpdated(r);
            org.IsActive = IsActive(r);
            org.LangLabelVariant = GetIntNull(r, "langLabelVariant");
            org.Encoded = GetEncoded(r);
            org.Decode();

            if (org.LangCode == null) org.LangCode = GC.LangCodeDefault;

            return org;
        }
    }
}
