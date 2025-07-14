using Common.DTO;
using Common.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using System.Diagnostics;
using GC = Backend.GlobalConstants;

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

        protected string GetLabel(string langKey, Dictionary<string, string> labels)
        {
            if (labels.ContainsKey(langKey))
                return labels[langKey];
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

        public async Task<List<ValDto>> ValidateFields<T,V>(List<T> dtos) 
            where V : ValidatorI<T>, new()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langDic = await _labelService.GetLangCodeDic(session);
            var validations = new List<ValDto>();

            //Check fields
            foreach (var dto in dtos)
            {
                var v = new V().Validate(dto, langDic);
                if (v.Status() != GC.ValStatusOk)
                    validations.Add(v);
            }

            return validations;
        }

        public async Task<List<ValDto>> ValidateCodesInDB<T,E>(List<T> dtos, List<E> codesInDb)
            where T : _BaseFieldsDto<T>
            where E : BaseEntity
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langDic = await _labelService.GetLangCodeDic(session);
            var validations = new List<ValDto>();

            foreach (var dto in dtos)
            {
                if (string.IsNullOrEmpty(dto.Code)) continue;

                foreach (var ent in codesInDb)
                {
                    if (ent.Id != dto.Id && ent.Code.Equals(dto.Code))
                    {
                        var vm = new ValMessage
                        {
                            Message = GetLabel("InvCE", langDic)
                        };
                        var v = new ValDto()
                        {
                            Id = dto.Id,
                            Code = ent.Code
                        };
                        v.Messages.Add(vm);

                        validations.Add(v);
                    }
                }
            }

            return validations;
        }

        public async Task<List<ValDto>> ValidateCodesNew<T>(List<T> dtos)
            where T : _BaseFieldsDto<T>
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langDic = await _labelService.GetLangCodeDic(session);
            var validations = new List<ValDto>();
            
            foreach (var dto in dtos)
            {
                if (string.IsNullOrEmpty(dto.Code) || !dto.IsNew()) continue;

                foreach (var dtoX in dtos)
                {
                    if (string.IsNullOrEmpty(dtoX.Code) || !dtoX.IsNew()) continue;

                    if (dtoX.Id != dto.Id && dtoX.Code.Equals(dto.Code))
                    {
                        var vm = new ValMessage
                        {
                            Message = GetLabel("InvCD", langDic)
                        };
                        var v = new ValDto()
                        {
                            Id = dto.Id,
                            Code = dto.Code,
                        };
                        v.Messages.Add(vm);

                        validations.Add(v);
                        var vX = new ValDto()
                        {
                            Id = dtoX.Id,
                            Code = dtoX.Code
                        };
                        vX.Messages.Add(vm);

                        validations.Add(vX);
                    }
                }
            }

            return validations;
        }

        [NonAction]
        public void ValidateCombine(List<ValDto> into, List<ValDto> from)
        {
            var intoMap = into.ToDictionary(v => v.Id);

            foreach (var valX in from)
            {
                if (intoMap.TryGetValue(valX.Id, out var existing))
                {
                    existing.Messages.AddRange(valX.Messages);
                }
                else
                {
                    into.Add(valX);
                    intoMap[valX.Id] = valX;
                }
            }
        }

        public async Task<IActionResult> Response(List<ValDto> validations)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langDic = await _labelService.GetLangCodeDic(session);

            var x = new _ResponseDto
            {
                Valid = false,
                StatusCode = GC.StatusCodeUnProcessable,
                ErrorMessage = GetLabel("InvR", langDic),
                Result = validations
            };
            return Ok(x);
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