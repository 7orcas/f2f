using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Backend.App.Machines
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly Serilog.ILogger _log;
        protected AuditServiceI _auditService;

        public BaseController(IServiceProvider serviceProvider)
        {
            _log = Log.Logger;

            // Create a scoped service provider
            using var scope = serviceProvider.CreateScope();
            _auditService = scope.ServiceProvider.GetRequiredService<AuditServiceI>();
        }

    }
}