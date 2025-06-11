using Backend.App.Machines;
using Backend.Base.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using GC = Backend.GlobalConstants;

/// <summary>
/// Audit of:
/// - all transactions in db
/// - logins / outs events
/// - no permission events
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Audit
{
    public class AuditService: AuditServiceI
    {
        protected readonly Serilog.ILogger _log;
        private readonly EntityServiceI _entityService;

        public AuditService(EntityServiceI entityService) 
        {
            _log = Log.Logger;
            _entityService = entityService;
        }

        public async Task<List<AuditList>> GetEvents(SessionEnt session)
        {
            List<AuditList> list = new List<AuditList>();
            await Sql.Run(
                    "SELECT a.*, z.xxx " +
                    "FROM base.Audit a " +
                    "LEFT JOIN base.zzz z ON z.id = a.userId ",
                    r => {
                        list.Add(new AuditList()
                        {
                            Id = Sql.GetId(r, "id"),
                            OrgId = Sql.GetId(r, "orgId"),
                            Source = Sql.GetInt(r, "source"),
                            EntityTypeId = Sql.GetInt(r, "entityTypeId"),
                            EntityId = Sql.GetIntNull(r, "entityId"),
                            UserId = Sql.GetInt(r, "userId"),
                            User = Sql.GetString(r, "xxx"),
                            Created = Sql.GetDateTime(r, "created"),
                            Crud = Sql.GetString(r, "crud"),
                            Details = Sql.GetString(r, "details")
                        });
                    });

            foreach (var a in list)
            { 
                a.EntityType = _entityService.GetEntityTypeName(a.EntityTypeId);
            }

            return list;
        }

        public void ReadEntity(SessionEnt session, int entityTypeId, int entityId)
        {
            Task.Run(async () =>
            {
                try
                {
                    LogAuditRecord(session, entityTypeId, entityId, GC.CrudRead, null);
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

        public void LogInOut(int sourceApp, int orgId, int loginId, int entityTypeId)
        {
            Task.Run(async () =>
            {
                try
                {
                    LogAuditRecord(sourceApp, orgId, loginId, entityTypeId, null, null, null);
                }
                catch (Exception ex)
                {
                    _log.Error("Audit Read:" + ex.Message);
                }
            });
        }

        private async void LogAuditRecord(SessionEnt session,
            int entityTypeId,
            int? entityId,
            string crud,
            string details)
        {
            LogAuditRecord(session.SourceApp,
                session.Org.Id,
                session.User.LoginId,
                entityTypeId,
                entityId,
                crud,
                details);
        }

        private async void LogAuditRecord(
            int sourceApp,
            int orgId,
            int loginId,
            int entityTypeId,
            int? entityId, 
            string crud, 
            string details)
        {
            await Sql.Execute(
                    "INSERT INTO base.Audit " +
                        "(orgId, source, entityTypeId, entityId, userId, crud, details) " +
                    "VALUES (" + 
                        orgId + "," +
                        sourceApp + "," +
                        entityTypeId + "," +
                        (entityId == null? "null" : entityId) + "," +
                        loginId + "," +
                        (crud == null ? "null" : "'" + crud + "'") + "," +
                        (details == null ? "null" : "'" + details + "'") + 
                        ")"
            );
        }

    }


}
