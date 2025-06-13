using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Base.Permission
{
    [TestClass]
    public class PermissionServiceTest : BaseTest
    {
        PermissionService service;

        public PermissionServiceTest()
        {
            service = new PermissionService(null, null);
        }

        [TestMethod]
        public async Task LoadPermissions()
        {
            var list = await service.LoadEffectivePermissionsInt(UserIdTest, OrgIdTest);
            Assert.AreEqual(9, list.Count);
        }


    }
}
