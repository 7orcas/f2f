using Common.DTO;

/// <summary>
/// Base validation utilities
/// Created: July 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Common.Validator
{
    public abstract class _BaseVal<T, D> where T : _BaseVal<T,D> where D : _BaseFieldsDto<D>
    {
        public _ValDto val {  get; } = new _ValDto();

        public _ValDto Validate(D dto, Dictionary<string, string> labels) 
        {
            if (dto.Code == null || 
                string.IsNullOrWhiteSpace(dto.Code) ||
                dto.Code.EndsWith(" "))
                val.AddError("code", GetLabel("InvF", labels));


            return val;
        }

        protected string GetLabel (string key, Dictionary<string, string> labels)
        {
            if (labels == null) return key;
            if (labels.TryGetValue(key, out var v))
                return v;
            return key;
        }

    }
}
