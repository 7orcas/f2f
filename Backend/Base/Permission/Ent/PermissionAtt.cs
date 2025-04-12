namespace Backend.Base.Permission.Ent
{
    [AttributeUsage (AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PermissionAtt : Attribute
    {
        public string Name { get; }
        public string Crud { get; }

        public PermissionAtt(string name)
        {
            Name = name;
        }
    }
}
