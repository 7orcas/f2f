using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;
using System.Runtime.ConstrainedExecution;

namespace Backend.Base.Permission
{
    [Authorize]
    [PermissionAtt(GC.PerPerm)]
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
            var permissions = await _PermissionService.GetPermissions(session);
            var permDic = _PermissionInitialiseService.GetPermissions();
            var langDic = await _labelService.GetLanguageLabelDic(session.UserConfig.LangCodeCurrent, session.Org.LangLabelVariant);

            var list = new List<UserRolePermissionDto>();
            foreach (var m in permissions)
            {
                var lk = "?";
                if (permDic.ContainsKey(m.PermissionNr))
                {
                    lk = (permDic[m.PermissionNr]).LangKey;
                    lk = GetLabel(lk, langDic);
                }

                list.Add(new UserRolePermissionDto
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
            var permissions = await _PermissionService.LoadEffectivePermissions(session);
            var permDic = _PermissionInitialiseService.GetPermissions();
            var langDic = await _labelService.GetLanguageLabelDic(session.UserConfig.LangCodeCurrent, session.Org.LangLabelVariant);

            var list = new List<PermissionDto>();

            foreach (var m in permissions)
            {
                if (!permDic.ContainsKey(m.Nr)) continue;
                var per = permDic[m.Nr];

                list.Add(new PermissionDto
                {
                    PermissionNr = per.Nr,
                    LangKey = GetLabel(per.LangKey, langDic),
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