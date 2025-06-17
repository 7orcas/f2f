
namespace Backend.Base.Audit
{
    public interface AuditServiceI
    {
        void ReadList(SessionEnt session, int entityTypeId, string query);
        void ReadEntity(SessionEnt session, int entityTypeId, long entityId);
        void LogInOut(int sourceApp, long orgId, long loginId, int entityTypeId);
        Task<List<AuditList>> GetEvents(SessionEnt session);
    }
}
