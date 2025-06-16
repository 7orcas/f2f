using GC = Backend.GlobalConstants;
using GCT = BackendTest.GlobalConstants;

namespace BackendTest
{
    public partial class BaseTest
    {
        public async Task<OrgEnt> GetOrgEnt()
        {
            var org = await orgService.GetOrg(GCT.OrgNr);
            return org;
        }


        public async Task<UserEnt> GetUserEnt()
        {
            var login = await loginservice.GetLogin(GCT.UserName);
            var org = await orgService.GetOrg(GCT.OrgNr);
            var user = await loginservice.InitialiseLogin(login, org, GC.AppClient);
            return user;
        }

    }
}
