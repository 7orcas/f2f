using Backend.Base.Audit;
using Microsoft.Data.SqlClient;

namespace Backend.Base;

public abstract partial class BaseService
{
    protected readonly Serilog.ILogger _log;
    protected AuditServiceI _auditService;

    public BaseService(IServiceProvider serviceProvider) 
    { 
        _log = Log.Logger;
        
        // Create a scoped service provider
        using var scope = serviceProvider.CreateScope();
        _auditService = scope.ServiceProvider.GetRequiredService<AuditServiceI>();
    }

    public T ReadBaseEntity<T>(SqlDataReader r) where T : BaseEntity, new()
    {
        var entity = new T();
        entity.OrgId = GetOrgId(r);
        entity.Id = GetId(r);
        entity.Code = GetCode(r);
        entity.Description = GetDescription(r);
        entity.Encoded = GetEncoded(r);
        //entity.Encoded = (string)r["Encoded"]; // Force Error, testing
        entity.Updated = GetUpdated(r);
        entity.IsActive = IsActive(r);

        return entity;
    }

    public void DelaySeconds (int seconds)
    {
        System.Threading.Thread.Sleep(seconds * 1000); 
    }

    
}
