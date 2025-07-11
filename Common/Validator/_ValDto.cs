using GC = Common.GlobalConstants;

/// <summary>
/// Dto validation message(s)
/// Created: July 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Common.Validator
{
    public class _ValDto
    {
        public List<ValMessage> Messages { get; set; }

        public int Status()
        {
            int s = GC.ValStatusOk;

            if (Messages == null) return s;
            foreach (ValMessage m in Messages)
            {
                if (m.Status == GC.ValStatusError)
                    return GC.ValStatusError;
            }
            return GC.ValStatusWarning;
        }

        public void AddWarning(string field, string message) => Add(field, message, GC.ValStatusWarning);
        public void AddError(string field, string message) => Add(field, message, GC.ValStatusError);

        private void Add(string field, string message, int status)
        {
            if (Messages == null)
                Messages = new List<ValMessage>();

            Messages.Add(new ValMessage
            {
                Status = status,
                Field = field,
                Message = message
            });
        }

    }

    public class ValMessage 
    { 
        public int Status { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
    }

}
