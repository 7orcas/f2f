using Common.DTO;

namespace Common.Validator
{
    public class RoleVal : _BaseVal<RoleVal, RoleDto>
    {
        private RoleDto dto;
        public RoleVal(RoleDto dto) 
        { 
            this.dto = dto;
        }

        public _ValDto Validate (Dictionary<string, string> labels) => base.Validate(dto, labels);

    }

}
