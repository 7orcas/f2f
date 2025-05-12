namespace Backend.Base.Audit.Ent
{
    [AttributeUsage (AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuditAtt : Attribute
    {
        public int AuditUserAction { get; }
        public int EntityTypeId { get; }

        public AuditAtt(int auditAction, int entityTypeId)
        {
            AuditUserAction = auditAction;
            EntityTypeId = entityTypeId;
        }
    }
}
