namespace Backend.Base.Permission.Ent
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAtt : Attribute
    {
        public string Name { get; }
        public PermissionAtt(string name)
        {
            Name = name;
        }
    }
}
