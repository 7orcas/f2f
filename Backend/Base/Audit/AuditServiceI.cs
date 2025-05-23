
namespace Backend.Base.Audit
{
    public interface AuditServiceI
    {
        void ReadList(SessionEnt session, int entityTypeId, string query);
        void ReadEntity(SessionEnt session, int entityTypeId, int entityId);
        void LogInOut(int sourceApp, int orgId, int loginId, int entity);
        Task<List<AuditList>> GetEvents(SessionEnt session);
    }
}
