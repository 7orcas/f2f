using Backend.App.Machines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Role
{
    [Authorize]
    [PermissionAtt("role")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private readonly RoleServiceI _RoleService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RoleService"></param>
        public RoleController(RoleServiceI RoleService)
        {
            _RoleService = RoleService;
        }

        [CrudAtt(GC.CrudRead)]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var roles = await _RoleService.GetUserRoles(session.User.LoginId, session.Org.Id);
            var list = new List<UserRoleDto>();

            foreach (var m in roles)
            {
                list.Add(new UserRoleDto
                {
                    RoleId = m.RoleId,
                    OrgId = m.OrgId,
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