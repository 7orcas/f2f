using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;
using CGC = Common.GlobalConstants;

namespace Backend.Base.Role
{
    [Authorize]
    [PermissionAtt(CGC.PerRole)]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private readonly RoleServiceI _RoleService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="RoleService"></param>
        public RoleController(IServiceProvider serviceProvider,
            RoleServiceI RoleService) : base(serviceProvider)
        {
            _RoleService = RoleService;
        }

        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeRole)]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var roles = await _RoleService.GetUserRoles(session);
            var list = new List<UserRoleDto>();

            foreach (var m in roles)
            {
                list.Add(new UserRoleDto
                {
                    RoleId = m.RoleId,
                    orgNr = m.orgNr.HasValue? m.orgNr.Value : GC.BaseorgNr,
                    Code = m.Code,
                    Description = m.Description,
                    Updated = m.Updated,
                    IsActive = m.IsActive,
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