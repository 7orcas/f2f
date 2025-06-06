namespace FrontendServer.Mods
{
    public class ModPageRoutes
    {
        static Dictionary<string, string> PageCodeDic; //page code, route

        //Routes
        public const string RouteMachines = "/machines";

        //Page Codes
        public const string PageMachines = "m01";
        

        private static readonly string[] PageCodeRoutes = {
            PageMachines,       RouteMachines,
        };


        public static bool IsRoute (string pageCode)
        {
            if (PageCodeDic == null)
                InitialiseRoutes();
            return PageCodeDic.ContainsKey(pageCode);
        }

        public static string? GetRoute(string pageCode)
        {
            if (PageCodeDic == null)
                InitialiseRoutes();
            if (IsRoute(pageCode))
                return PageCodeDic[pageCode];
            return null;
        }

        private static void InitialiseRoutes()
        {
            PageCodeDic = new Dictionary<string, string>();
            for (int i = 0; i < PageCodeRoutes.Length; i += 2)
            {
                PageCodeDic.Add(PageCodeRoutes[i], PageCodeRoutes[i + 1]);
            }
        }

    }
}
