using GC = Backend.GlobalConstants;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Load organisation configurations on startup
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Config
{
    public class OrgConfigInitialiseService : BaseService, OrgConfigInitialiseServiceI
    {
        private readonly IMemoryCache _memoryCache;
        
        public OrgConfigInitialiseService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache)
            : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// Load all orgainsational configurations and cache 
        /// </summary>
        /// <returns></returns>
        public async Task InitialiseOrgConfigs()
        {
            var orgs = new List<OrgEnt>();
            await Sql.Run(
                    "SELECT * FROM base.org",
                    r => {
                        orgs.Add(new OrgEnt
                        {
                            Id = GetId(r),
                            Nr = GetNr(r),
                            LangCode = GetStringNull(r, "langCode"),
                            LangLabelVariant = GetIntNull(r, "langLabelVariant"),
                            Encoded = GetEncoded(r)
                        });
                    }
                );

            var langCodes = new List<LangCode>();
            await Sql.Run(
                    "SELECT * FROM base.LangCode",
                    r => {
                        langCodes.Add(new LangCode()
                        {
                            Code = GetCode(r),
                            IsActive = IsActive(r),
                        });
                    }
                );

            //Config all organisations
            foreach (var org in orgs)
            {
                org.Decode();
                if (org.LangCode == null) org.LangCode = GC.LangCodeDefault;

                //Config languages
                var lConfigs = new List<LanguageConfig>();
                foreach (var l in org.Languages)
                {
                    var v = ValidateLanguage(org, l, langCodes);
                    if (v != null) lConfigs.Add(v);
                }

                //Config Org
                var oConfig = new OrgConfig()
                {
                    OrgId = org.Id,
                    LangCodeDefault = org.LangCode,
                    IsLangCodeEditable = org.IsLangCodeEditable,
                    Languages = lConfigs
                };

                _memoryCache.Set(GC.CacheKeyOrgConfigPrefix + org.Id, oConfig);
            }
        }

        private LanguageConfig? ValidateLanguage(OrgEnt org, string langCode, List<LangCode> langCodes)
        {
            //Lang Code must exist in database
            var lc = langCodes.Find(c => c.Code == langCode);
            if (lc == null) return null;

            return new LanguageConfig()
            {
                LangCode = langCode,
                IsActive = lc.IsActive,
                IsUpdateable = org.IsLangCodeEditable,
            };
        }
    }
}
