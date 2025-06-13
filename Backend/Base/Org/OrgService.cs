using Microsoft.Data.SqlClient;

namespace Backend.Base.Org
{
    public class OrgService: BaseService, OrgServiceI
    {
        public OrgService(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public async Task<OrgEnt> GetOrg(int nr)
        {
            var l = new OrgEnt();
            try
            {
                await Sql.Run(
                    "SELECT * FROM base.org "
                    + "WHERE nr = @nr ",
                    r => {
                        l.Id = GetId(r);
                        l.Nr = GetNr(r);
                        l.Code = GetCode(r);
                        l.Description = GetDescription(r);
                        l.Updated = GetUpdated(r);
                        l.IsActive = IsActive(r);
                        l.LangLabelVariant = GetIntNull(r, "langLabelVariant");
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
