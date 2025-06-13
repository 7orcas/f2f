using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest
{
    public class BaseTest
    {
        private bool initialised = false;
        public const int UserIdTest = 2;
        public const int OrgIdTest = 1;
        public readonly IMemoryCache _memoryCache;
        private bool TestUserInitialised = false;

        public BaseTest() 
        {
            if (initialised) return;
            initialised = true;
            AppSettings.DBMainConnection = "Server=np:localhost;Database=blue;TrustServerCertificate=True;Authentication=Active Directory Integrated;";
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            SetupTestDb();
        }


        public async Task SetupTestDb()
        {



        }



    }
}
