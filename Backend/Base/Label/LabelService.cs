using Backend.Base.Label.Ent;
using Microsoft.Data.SqlClient;

namespace Backend.Base.Label
{
    public class LabelService : BaseService, LabelServiceI
    {
        public LabelService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<LangLabel>> GetLabels(string langCode)
        {
            return await GetLabelList(langCode, null);
        }

        private async Task<List<LangLabel>> GetLabelList(string langCode, int? hardCodedNr)
        {
            string sql = "SELECT l.id, l.langKeyId, l.langCode, k.code AS kCode, l.hardCodedNr, l.code, l.tooltip, l.updated, l.isActive " +
                            "FROM base.langLabel l " +
                            "INNER JOIN base.langKey k ON k.id = l.langKeyId " + 
                            "WHERE l.langCode = '" + langCode + "' ";

            if (hardCodedNr.HasValue) 
                sql += "AND l.hardCodedNr = " + hardCodedNr;

            //Step 1: get labels for passed in lang code and org
            var list = new List<LangLabel>();
            await Sql.Run(sql,
                    r => {
                        list.Add(new LangLabel()
                        {
                            Id = Sql.GetId(r, "id"),
                            LangKeyId = Sql.GetId(r, "langKeyId"),
                            LangCode = Sql.GetString(r, "langCode"),
                            LabelCode = Sql.GetString(r, "kCode"),
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
