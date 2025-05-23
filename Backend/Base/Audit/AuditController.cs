using Backend.App.Machines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Audit
{
    [Authorize]
    [PermissionAtt("audit")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : BaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="AuditService"></param>
        public AuditController(IServiceProvider serviceProvider,
            AuditServiceI AuditService) : base(serviceProvider)
        {
        }


        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeAudit)]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var events = await _auditService.GetEvents(session);
            var list = new List<AuditDto>();

            foreach (var e in events)
            {
                list.Add(new AuditDto
                {
                    Id = e.Id,
                    OrgId = e.OrgId,
                    Source = e.Source,
                    EntityType = e.EntityType,
                    EntityId = e.EntityId,
                    User = e.User,
                    Created = e.Created,
                    Crud = e.Crud,
                    Details = e.Details,
                });
            }

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }


    }
}