
namespace Backend.Base.Org
{
    public interface OrgServiceI
    {
        Task<OrgEnt> GetOrg(int nr);
        Task<List<OrgEnt>> GetOrgs();
        Task UpdateOrg(OrgEnt org);
    }
}
