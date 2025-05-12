
namespace Backend.Base.Audit
{
    public interface AuditServiceI
    {
        void ReadList(SessionEnt session, int entityTypeId, string query);
        void ReadEntity(SessionEnt session, int entityTypeId, int entityId);
        Task<List<AuditList>> GetEvents(SessionEnt session);
    }
}
