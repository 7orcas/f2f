namespace Backend.Base.Session.Ent
{
    public class SessionEnt
    {
        public string Key { get; set; }
        public OrgEnt Org { get; set; }
        public LoginEnt Login { get; set; }


        public bool IsPermission (string permission)
        {
            if (permission.Equals("machine"))
                return true;
            return false;
        }


    }
}
