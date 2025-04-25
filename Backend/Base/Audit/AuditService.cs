using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Audit
{
    public class AuditService: AuditServiceI
    {
        protected readonly Serilog.ILogger _log;

        public AuditService() 
        {
            _log = Log.Logger;
        }

        public async Task<List<AuditList>> GetEvents(SessionEnt session)
        {

            ReadList(session, GC.EntityAudit, null);

            List<AuditList> list = new List<AuditList>();
            await Sql.Run(
                    "SELECT * FROM base.Audit a",
                    r => {
                        list.Add(new AuditList()
                        {
                            Id = Sql.GetId(r, "id"),
                            OrgId = Sql.GetId(r, "orgId"),
                            Source = Sql.GetInt(r, "source"),
                            Entity = Sql.GetInt(r, "entity"),
                            EntityId = Sql.GetInt(r, "entityId"),
                            UserId = Sql.GetInt(r, "userId"),
                            Created = Sql.GetDateTime(r, "created"),
                            Crud = Sql.GetString(r, "crud"),
                            Details = Sql.GetString(r, "details")
                        });
                    });



            return list;
        }

        public void ReadEntity(SessionEnt session, int entity, int entityId)
        {
            Task.Run(async () =>
            {
                try
                {
                    LogAuditRecord(session, entity, entityId, GC.CrudRead, null);
                }
                catch (Exception ex) 
                { 
                    _log.Error("Audit Read:" + ex.Message);
                }
            });
        }

        public void ReadList(SessionEnt session, int entity, string query)
        {
            Task.Run(async () =>
            {
                try
                {
                    LogAuditRecord(session, entity, null, GC.CrudReadList, query);
                }
                catch (Exception ex)
                {
                    _log.Error("Audit Read:" + ex.Message);
                }
            });
        }

        private async void LogAuditRecord(SessionEnt session, int entity, int? entityId, string crud, string details)
        {
            await Sql.Execute(
                    "INSERT INTO base.Audit " +
                        "(orgId, source, entity, entityId, userId, crud, details) " +
                    "VALUES (" + 
                        session.Org.Id + "," +
                        session.SourceApp + "," +
                        entity + "," +
                        (entityId == null? "null" : entityId) + "," +
                        session.User.LoginId + "," +
                        "'" + crud + "'," +
                        (details == null ? "null" : "'" + details + "'") + 
                        ")"
            );
        }

    }


}
