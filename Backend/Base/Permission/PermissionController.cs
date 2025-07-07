using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;
using CGC = Common.GlobalConstants;

namespace Backend.Base.Permission
{
    [Authorize]
    [PermissionAtt(CGC.PerPerm)]
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
            var PermDic = _PermissionInitialiseService.GetPermissions();

            var list = new List<RolePermissionDto>();
            foreach (var m in Permissions)
            {
                var lk = "?";
                if (PermDic.ContainsKey(m.PermissionNr))
                    lk = (PermDic[m.PermissionNr]).LangKey;

                list.Add(new RolePermissionDto
                {
                    OrgNr = m.OrgNr,
                    Role = m.Role,
                    PermissionNr = m.PermissionNr,
                    LangKey = lk,
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
            var PermDic = _PermissionInitialiseService.GetPermissions();

            var list = new List<PermissionDto>();

            foreach (var m in Permissions)
            {
                if (!PermDic.ContainsKey(m.Nr)) continue;
                var per = PermDic[m.Nr];

                list.Add(new PermissionDto
                {
                    PermissionNr = per.Nr,
                    LangKey = per.LangKey,
                    Crud = m.Crud
                });
            }

            list = list.OrderBy(r => r.PermissionNr).ToList();

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }

    }
}