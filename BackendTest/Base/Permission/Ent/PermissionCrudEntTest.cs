using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Base.Permission.Ent
{
    [TestClass]
    public class PermissionCrudEntTest
    {
        [TestMethod]
        public void AddCrud()
        {
            var e = new PermissionCrudEnt();

            string[,] test = new string[,]
            {
                {"u",    "ud", "ud"},
                {"crud", "ud", "crud"},
                {"ud",   "c",  "cud"},
                {"cd",   "ur", "crud"},
                {"drc",  "drc","crd"},
            };

            for (int row = 0; row < test.GetLength(0); row++)
            {
                e.Crud = test[row, 0];
                e.AddCrud(test[row, 1]);
                Assert.AreEqual(e.Crud, test[row, 2]);
            }

        }
    }
}
