
using Backend.App.Login.Ent;
using Microsoft.Data.SqlClient;

namespace Backend.App.Login
{
    public class LoginService: BaseService, LoginServiceI
    {
        public async Task<Backend.App.Login.Ent.Login> GetLogin(string userid)
        {
            var l = new Backend.App.Login.Ent.Login();
            try
            {
                //ToDo Log
                if (!Sql.ValidateParameter(userid))
                    return l;

                await Sql.Run(
                    "SELECT * FROM _base.zzz "
                    + "WHERE xxx = @userid ",
                    r => {
                        l.Id = GetId(r, "id");
                        l.Userid = GetString(r, "xxx");
                        l.Password = GetString(r, "yyy");
                        l.Orgs = GetString(r, "orgs");
                        l.Attempts = GetId(r, "attempts");
                        l.Lastlogin = GetDateTime(r, "lastlogin");
                        l.IsActive = GetBoolean(r, "isActive");
                    },
                    new SqlParameter("@userid", userid)
                );
                return l;
            }
            catch 
            {
                return l;
            }
        }

        public async Task<bool> InitialiseLogin(Backend.App.Login.Ent.Login l)
        {
            await SetAttempts(l.Id, 0);

            return true;
        }

        public async Task<bool> IncrementAttempts(Backend.App.Login.Ent.Login l)
        {
            return await SetAttempts(l.Id, ++l.Attempts);
        }

        private async Task<bool> SetAttempts(int id, int attempts)
        {
            await Sql.Execute(
                   "UPDATE _base.zzz "
                   + "SET Attempts = " + attempts + " "
                   + "WHERE id = " + id
               );
            return true;
        }

        //ToDO Labels
        public string? Validate (Backend.App.Login.Ent.Login l, int org)
        {
            if (l == null
                || string.IsNullOrEmpty(l.Orgs)) 
                return "Invalid Organisation";

            List<int> numbers = l.Orgs
                .Split(',')               
                .Select(int.Parse)        
                .ToList();                

            if (!numbers.Contains(org))
                return "Invalid Organisation";

            return null;
        }
    }
}
