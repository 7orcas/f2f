using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Label
{
    public class LabelService : BaseService, LabelServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public LabelService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache) 
            : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Dictionary<string, LangLabel>> GetLanguageLabelDic(string langCode, int? variant)
        {
            var dic = _memoryCache.Get<Dictionary<string, LangLabel>>(CacheKey(langCode, variant));
            if (dic != null) return dic;

            await GetLanguageLabelList(langCode, variant);
            return _memoryCache.Get<Dictionary<string, LangLabel>>(CacheKey(langCode, variant));
        }

        private async Task SetLanguageLabelDic(string langCode, int? variant, List<LangLabel> list)
        {
            var dic = list.ToDictionary(x => x.LangKeyCode, x => x);
            _memoryCache.Set(CacheKey(langCode, variant), dic);
        }

        private string CacheKey(string langCode, int? variant)
        {
            return GC.CacheKeyLabelPrefix + langCode + (variant.HasValue ? variant : 0);
        }

        /*
         * Get language list for passed in language code (eg 'en')
         */
        public async Task<List<LangLabel>> GetLanguageLabelList(string langCode, int? variant)
        {
            var dic = _memoryCache.Get<Dictionary<string, LangLabel>>(CacheKey(langCode, variant));
            if (dic != null) return dic.Values.ToList(); 

            var list = await GetLabelList(null, langCode, null, null, null);
            if (variant.HasValue)
            {
                try
                {
                    var dict = list.ToDictionary(x => x.LangKeyCode, x => x);

                    var listX = await GetLabelList(null, langCode, null, null, variant);
                    foreach (var label in listX) 
                        dict[label.LangKeyCode] = label;
                    
                    list = dict.Values.ToList();
                }
                catch 
                {
                    _log.Error("Can't get language variants");
                }
            }

            await SetLanguageLabelDic(langCode, variant, list);
            return list;
        }

        /*
         * Get language label for passed in language code id
         */
        public async Task<LangLabel> GetLanguageLabel(int id)
        {
            List <LangLabel> list = await GetLabelList(id, null, null, null, null);
            if (list.Count == 1) return list[0];
            return null;
        }

        public async Task<List<LangLabel>> GetAllLanguageLabels()
        {
            return await GetLabelList(null, null, null, null, null);
        }

        /**
         * Return related labels, ie for a given label key get all languages
         */
        public async Task<List<LangLabel>> GetRelatedLabels(string langKeyCode, List<string> langCodes)
        {
            return await GetLabelList(null, null, langKeyCode, langCodes, null);
        }

        /*
         * Get language key for passed in language key code 
         */
        public async Task<LangKey> GetLanguageKey(string langKeyCode)
        {
            var k = null as LangKey;
            await Sql.Run(
                   "SELECT * FROM base.langKey WHERE code = @langKeyCode",
                   r => {
                       k = new LangKey {
                           Id = GetId(r),
                           Pack = GetStringNull(r, "pack"),
                           Code = GetCode(r),
                           Description = GetDescription(r),
                           Updated = GetUpdated(r),
                       };
                   },
                   new SqlParameter("@langKeyCode", langKeyCode)
               );

            return k;
        }


        private async Task<List<LangLabel>> GetLabelList(
            int? id, 
            string? langCode,
            string? langKeyCode,
            List<string> langCodes,
            int? variant)
        {
            string sql = "SELECT l.id, l.langKeyId, l.langCode, k.code AS kCode, l.variant, l.code, l.tooltip, l.updated " +
                            "FROM base.langLabel l " +
                            "INNER JOIN base.langKey k ON k.id = l.langKeyId ";

            string sqlWhere = "";

            if (id != null)
                sqlWhere = "WHERE l.id = " + id + " ";
            else
            {
                sqlWhere = "WHERE l.variant " + (variant.HasValue ? " = " + variant : " IS NULL") + " ";

                if (!string.IsNullOrEmpty(langCode))
                    sqlWhere += "AND l.langCode = '" + langCode + "' ";

                else if (langCodes  != null)
                {
                    sqlWhere += "AND (";
                    for (int i = 0; i < langCodes.Count; i++)
                        sqlWhere += (i==0?"":" OR ") + "l.langCode = '" + langCodes[i] + "'";
                    sqlWhere += ") ";
                }

                if (!string.IsNullOrEmpty(langKeyCode))
                    sqlWhere = "AND k.code = '" + langKeyCode + "' ";

            }
            sql += sqlWhere;

            //TESTING
            //sql += " and k.code = 'Machines' ";

            //Step 1: get labels for passed in lang code and org
            var list = new List<LangLabel>();
            await Sql.Run(sql,
                    r => {
                        list.Add(new LangLabel()
                        {
                            Id = GetId(r),
                            LangKeyId = GetId(r, "langKeyId"),
                            LangKeyCode = GetString(r, "kCode"),
                            LangCode = GetString(r, "langCode"),
                            Variant = GetIntNull(r, "variant"),
                            Code = GetCode(r),
                            Tooltip = GetStringNull(r, "tooltip"),
                            Updated = GetUpdated(r)
                        });
                    }
            );

            return list;
        }

       

        public async Task<bool> SaveLabel(LangLabel label)
        {
            string sql = "";

            if (label.Id > 0)
                sql = "UPDATE  base.langLabel SET "
                    + "code = '" + label.Code + "', "
                    + (label.Tooltip != null ? "tooltip = '" + label.Tooltip + "'" : "Tooltip = NULL" ) + " "
                    + "WHERE id = " + label.Id;
            else
                sql = "INSERT INTO  base.langLabel "
                    + "(langKeyId, langCode, variant, code, tooltip) "
                    + " VALUES ("
                    + label.LangKeyId + ","
                    + "'" + label.LangCode + "',"
                    + (label.Variant != null ? label.Variant : "NULL") + ","
                    + "'" + label.Code + "',"
                    + (label.Tooltip != null ? "'" + label.Tooltip + "'" : "NULL")
                    + ")";

            await Sql.Execute(sql);
            return true;
        }

    }
}
