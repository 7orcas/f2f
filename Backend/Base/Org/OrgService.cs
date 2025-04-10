
using Microsoft.Data.SqlClient;

namespace Backend.Base.Org
{
    public class OrgService: BaseService, OrgServiceI
    {
        public async Task<OrgEnt> GetOrg(int nr)
        {
            var l = new OrgEnt();
            try
            {
                await Sql.Run(
                    "SELECT * FROM base.org "
                    + "WHERE nr = @nr ",
                    r => {
                        l.Id = GetId(r, "id");
                        l.Nr = GetId(r, "nr");
                        l.Code = GetString(r, "code");
                        l.Description = GetString(r, "descr");
                        l.Updated = GetDateTime(r, "updated");
                        l.IsActive = GetBoolean(r, "isActive");
                    },
                    new SqlParameter("@nr", nr)
                );
                return l;
            }
            catch 
            {
                return l;
            }
        }

       
    }
}
