namespace Backend
{
    /// <summary>
    /// Application wide settings
    /// Read in on start up from appsettings.json
    /// </summary>
    public class AppSettings
    {
        public static string DBMainConnection { get; set; }
        public static int MaxGetTokenCalls { get; set; }
        public static int CacheExpirationAddSeconds { get; set; }
        public static int CacheExpirationGetSeconds { get; set; }
    }
}
