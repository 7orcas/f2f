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
        private readonly PermissionInitialiseServiceI _PermissionInitialiseService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="RoleService"></param>
        public RoleController(IServiceProvider serviceProvider,
            RoleServiceI RoleService,
            PermissionInitialiseServiceI PermissionInitialiseService) 
            : base(serviceProvider)
        {
            _RoleService = RoleService;
            _PermissionInitialiseService = PermissionInitialiseService;
        }

        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeRole)]
        [HttpGet("userroles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var roles = await _RoleService.GetUserRoles(session);
            var list = new List<UserRoleDto>();

            foreach (var m in roles)
            {
                list.Add(new UserRoleDto
                {
                    RoleId = m.RoleId,
                    orgNr = m.orgNr.HasValue? m.orgNr.Value : GC.BaseOrgNr,
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

        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeRole)]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var roles = await _RoleService.GetRoles(session);
            var list = new List<RoleDto>();

            foreach (var m in roles)
                list.Add(LoadDto<RoleDto>(m));

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }

        /// <summary>
        /// Get Org
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)] //ToDo
        [AuditListAtt(GC.EntityTypeRole)]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRole(long id)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langDic = await _labelService.GetLanguageLabelDic(session.UserConfig.LangCodeCurrent, session.Org.LangLabelVariant);
            var permDic = _PermissionInitialiseService.GetPermissions();
            var permList = permDic.Values.OrderBy(v => v.Nr).ToList();

            var ent = await _RoleService.GetRole(id);
            var dto = LoadDto<RoleDto>(ent);
            dto.RolePermissions = new List<RolePermissionDto>();

            foreach(var perm in permList)
            {
                var rp = ent.RolePermissions.Find(r => r.PermissionNr == perm.Nr);
                var lk = GetLabel(perm.LangKey, langDic);
                
                if (rp  != null)
                    dto.RolePermissions.Add(new RolePermissionDto {
                        Id = rp.Id,
                        RoleId = rp.RoleId,
                        PermissionNr = rp.PermissionNr,
                        Permission = lk,
                        Crud = rp.Crud,
                        Updated = rp.Updated,
                        IsActive = rp.IsActive,
                    });
                else
                    dto.RolePermissions.Add(new RolePermissionDto
                    {
                        Id = GC.NewRecordId,
                        RoleId = ent.Id,
                        PermissionNr = perm.Nr,
                        Permission = lk,
                        Crud = ""
                    });
            }

            var r = new _ResponseDto
            {
                SuccessMessage = "Config Ok",
                Result = dto
            };

            return Ok(r);
        }


    }
}