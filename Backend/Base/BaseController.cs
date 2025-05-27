using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Backend.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly Serilog.ILogger _log;
        protected AuditServiceI _auditService;
        protected LabelServiceI _labelService;

        public BaseController(IServiceProvider serviceProvider)
        {
            _log = Log.Logger;

            // Create a scoped service provider
            using var scope = serviceProvider.CreateScope();
            _auditService = scope.ServiceProvider.GetRequiredService<AuditServiceI>();
            _labelService = scope.ServiceProvider.GetRequiredService<LabelServiceI>();
        }

    }
}