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
            service = new PermissionService();
        }

        [TestMethod]
        public async Task LoadPermissions()
        {
            var list = await service.LoadEffectivePermissions(UserIdTest, OrgIdTest);

            Assert.AreEqual(8, list.Count);
        }


        [TestMethod]
        public void test()
        {
            var r = service.test();
            Assert.IsNotNull(r);
            Assert.AreEqual(r, "TEST OK");

        }

    }
}
