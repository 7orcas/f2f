using Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;

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

        protected string GetLabel (string langKey, Dictionary<string, LangLabel> labels)
        {
            if (labels.ContainsKey(langKey)) 
                return (labels[langKey]).Code;
            return langKey;
        }

        protected bool IsSame (int? i1, int? i2)
        {
            if (i1 == null && i2 == null) return true;
            if (i1 == null || i2 == null) return false;
            return i1 == i2;
        }

        protected T LoadDto<T>(BaseEntity e) where T : _BaseFieldsDto<T>, new()
        {
            var dto = new T();
            dto.Id = e.Id;
            dto.orgNr = e.OrgNr;
            dto.Code = e.Code;
            dto.Description = e.Description;
            dto.Updated = e.Updated;
            dto.IsActive = e.IsActive;
            return dto;
        }

        protected void LogEvent(LogEventLevel level, string message, SessionEnt sessionEnt)
        {
            var logstring = "";
            try
            {
                var stackFrame = new StackTrace().GetFrame(1); // 0 = current method, 1 = caller
                var method = stackFrame.GetMethod();
                logstring = method.DeclaringType?.FullName;
               // logstring += "." + method.DeclaringType?.Name;
            }
            catch { }

            if (sessionEnt != null)
                logstring += ", SessionKey: " + sessionEnt.Key;

            logstring = message + ": " + logstring;

            if (level == LogEventLevel.Debug)
                _log.Debug(logstring);

            if (level == LogEventLevel.Error)
                _log.Error(logstring);

            if (level == LogEventLevel.Warning)
                _log.Warning(logstring);
        }

    }
}