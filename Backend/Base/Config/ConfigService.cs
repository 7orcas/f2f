using Backend.Base.Config.Ent;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using GC = Backend.GlobalConstants;

/// <summary>
/// Organisational and User configurations
/// Controls processing and client displays
/// Created: 
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Config
{
    public class ConfigService : BaseService, ConfigServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public ConfigService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache) 
            : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Created at Login Time
        /// </summary>
        /// <param name="user"></param>
        /// <param name="org"></param>
        /// <param name="requestedLangCode"></param>
        /// <returns></returns>
        public UserConfig CreateUserConfig(UserEnt user, OrgEnt org, string? requestedLangCode)
        {
            var orgConfig = _memoryCache.Get<OrgConfig>(GC.CacheKeyOrgConfigPrefix + org.Id);
            var userLangCode = ValidateLanguageCode(user, orgConfig, requestedLangCode);

            var userConfig = new UserConfig()
            {
                OrgId = org.Id,
                LangCodeCurrent = userLangCode,
                Languages = CreateClientLanguages(user, orgConfig, userLangCode)
            };

            return userConfig;
        }

        /// <summary>
        /// Make sure the user is using a valid language code
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orgConfig"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        private string ValidateLanguageCode(UserEnt user, OrgConfig orgConfig, string? langCode)
        {
            //Is user using the default org lang code?
            if (langCode == null) langCode = orgConfig.LangCodeDefault;
            if (langCode.Equals(orgConfig.LangCodeDefault)) return langCode;

            //Is the user's lang code used by the organisation (and active if required)?
            foreach (var lang in orgConfig.Languages)
                if (langCode.Equals(lang.LangCode))
                {
                    if (user.IsService) return langCode;
                    if (user.IsActiveLanguageAdmin && lang.IsActive) return langCode;
                }
                    
            return orgConfig.LangCodeDefault;
        }


        /// <summary>
        /// Get list of language codes the client is able to view and edit
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orgConfig"></param>
        /// <param name="userLangCode"></param>
        /// <returns></returns>
        private List<LanguageConfig> CreateClientLanguages(UserEnt user, OrgConfig orgConfig, string userLangCode)
        {
            var langList = new List<LanguageConfig>();

            if (!user.IsLanguageAdmin()) return langList;
            if (!user.IsService && !orgConfig.IsLangCodeEditable) return langList;

            foreach (var cl in orgConfig.Languages)
            {
                var up = user.IsService;
                if (!up && cl.LangCode.Equals(userLangCode)) up = cl.IsActive;
                if (!up && user.IsActiveLanguageAdmin) up = cl.IsActive;

                langList.Add(new LanguageConfig { 
                    LangCode = cl.LangCode,
                    IsActive = user.IsService || cl.IsActive,
                    IsEditable = up
                });
            }

            //Sort list
            langList = langList.OrderByDescending(x => x.LangCode).ToList();
            var dl = langList.Find(x => x.LangCode == userLangCode);
            
            var langListX = new List<LanguageConfig>();
            if (dl != null) 
                langListX.Add(dl);

            foreach (var l in langList)
                if (l.LangCode != userLangCode)
                    langListX.Add(l);

            return langListX;
        }

    }
}
