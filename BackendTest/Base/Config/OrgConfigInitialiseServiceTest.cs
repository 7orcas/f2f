using Backend.Base.Entity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC = Backend.GlobalConstants;

namespace BackendTest.Base.Config
{
    [TestClass]
    public class OrgConfigInitialiseServiceTest : BaseTest
    {
        OrgConfigInitialiseService service;

        public OrgConfigInitialiseServiceTest()
        {
            service = new OrgConfigInitialiseService(null, _memoryCache);
        }

        [TestMethod]
        public async Task InitialiseOrgConfigs()
        {
            await service.InitialiseOrgConfigs();

            var orgConfig = _memoryCache.Get<OrgConfig>(GC.CacheKeyOrgConfigPrefix + 1);

        }


    }
}
