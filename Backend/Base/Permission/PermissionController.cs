using Backend.App.Machines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;
using CGC = Common.GlobalConstants;

namespace Backend.Base.Permission
{
    [Authorize]
    [PermissionAtt(CGC.PermPerm)]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : BaseController
    {
        private readonly PermissionInitialiseServiceI _PermissionInitialiseService;
        private readonly PermissionServiceI _PermissionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="PermissionService"></param>
        public PermissionController(IServiceProvider serviceProvider,
            PermissionInitialiseServiceI PermissionInitialiseService,
            PermissionServiceI PermissionService) : base(serviceProvider)
        {
            _PermissionInitialiseService = PermissionInitialiseService;
            _PermissionService = PermissionService;
        }

        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypePermission)]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var Permissions = await _PermissionService.GetPermissions(session);
            
            var list = new List<RolePermissionDto>();
            foreach (var m in Permissions)
            {
                list.Add(new RolePermissionDto
                {
                    OrgId = m.OrgId,
                    Role = m.Role,
                    Permission = m.Permission,
                    Crud = m.Crud
                });
            }

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }

        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypePermissionEffect)]
        [HttpGet("listeffective")]
        public async Task<IActionResult> GetEffective()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var Permissions = await _PermissionService.LoadEffectivePermissions(session);
            var PermList = _PermissionInitialiseService.GetPermissions();

            var list = new List<PermissionDto>();

            foreach (var m in Permissions)
            {
                var per = PermList.FirstOrDefault(p => p.Permission == m.Permission);
                if (per == null) continue;

                list.Add(new PermissionDto
                {
                    Permission = per.Permission,
                    Crud = m.Crud
                });
            }

            list = list.OrderBy(r => r.Permission).ToList();

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }

    }
}