using Backend.Base.Label.Ent;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Backend.Base.Label
{
    public class LabelService : BaseService, LabelServiceI
    {
        public LabelService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /*
         * Get language list for passed in language code (eg 'en')
         */
        public async Task<List<LangLabel>> GetLanguageLabelList(string langKeyCode)
        {
            return await GetLabelList(null, langKeyCode, null);
        }

        /*
         * Get language label for passed in language code id
         */
        public async Task<LangLabel> GetLanguageLabel(int id)
        {
            List <LangLabel> list = await GetLabelList(id, null, null);
            if (list.Count == 1) return list[0];
            return null;
        }

        public async Task<List<LangLabel>> GetAllLanguageLabels()
        {
            return await GetLabelList(null, null, null);
        }


        private async Task<List<LangLabel>> GetLabelList(int? id, string? langKeyCode, int? hardCodedNr)
        {
            string sql = "SELECT l.id, l.langKeyId, l.langCode, k.code AS kCode, l.hardCodedNr, l.code, l.tooltip, l.updated, l.isActive " +
                            "FROM base.langLabel l " +
                            "INNER JOIN base.langKey k ON k.id = l.langKeyId ";
            string sqlWhere = "";


            if (id != null)
                sqlWhere = "WHERE l.id = " + id + " ";
            else if (!string.IsNullOrEmpty(langKeyCode))
                sqlWhere = "WHERE l.langCode = '" + langKeyCode + "' ";

            sqlWhere += (sqlWhere.Length > 0 ? "AND " : "WHERE ");

            if (hardCodedNr.HasValue) 
                sqlWhere += "l.hardCodedNr = " + hardCodedNr;
            else
                sqlWhere += "l.hardCodedNr IS NULL";

            sql += sqlWhere;

            //Step 1: get labels for passed in lang code and org
            var list = new List<LangLabel>();
            await Sql.Run(sql,
                    r => {
                        list.Add(new LangLabel()
                        {
                            Id = Sql.GetId(r, "id"),
                            LangKeyId = Sql.GetId(r, "langKeyId"),
                            LangKeyCode = Sql.GetString(r, "kCode"),
                            LangCode = Sql.GetString(r, "langCode"),
                            HardCodedNr = Sql.GetIntNull(r, "hardCodedNr"),
                            Code = Sql.GetString(r, "code"),
                            Tooltip = Sql.GetString(r, "tooltip"),
                            Updated = Sql.GetDateTime(r, "updated"),
                            IsActive = Sql.GetBoolean(r, "isActive")
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
                    + "(langKeyId, langCode, hardCodedNr, code, tooltip) "
                    + " VALUES ("
                    + label.LangKeyId + ","
                    + "'" + label.LangCode + "',"
                    + (label.HardCodedNr != null ? label.HardCodedNr : "NULL") + ","
                    + "'" + label.Code + "',"
                    + (label.Tooltip != null ? "'" + label.Tooltip + "'" : "NULL")
                    + ")";

            await Sql.Execute(sql);
            return true;
        }

    }
}
