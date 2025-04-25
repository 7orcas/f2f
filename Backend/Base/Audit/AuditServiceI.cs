using Backend.App.Machines.Ent;

namespace Backend.Base.Audit
{
    public interface AuditServiceI
    {
        void ReadList(SessionEnt session, int entity, string query);
        void ReadEntity(SessionEnt session, int entity, int entityId);
    }
}
