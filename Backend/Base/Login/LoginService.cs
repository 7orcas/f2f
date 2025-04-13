using Azure.Core;
using Backend.Base.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.Base.Login
{
    /// <summary>
    /// Manage login process for user
    /// Note a user can have multiple sessions open
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>March 5, 2025</created>
    /// <license>**Licence**</license>
    public class LoginService: BaseService, LoginServiceI
    {
        private readonly PermissionServiceI _permissionService;

        public LoginService (PermissionServiceI permissionService)
        {
            _permissionService = permissionService;
        }


        public async Task<LoginEnt> GetLogin(string userid)
        {
            var l = new LoginEnt();
            try
            {
                //ToDo Log
                if (!Sql.ValidateParameter(userid))
                    return l;

                await Sql.Run(
                    "SELECT * FROM base.zzz "
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

        public async Task<LoginErr> Validate (LoginEnt l, string password, int org)
        {
            if (l.Id == 0)
                return new LoginErr
                {
                    Message = "Invalid Username and/or Password."
                };

            await IncrementAttempts(l);

            if (string.IsNullOrEmpty(password) || !password.Equals(l.Password))
                return new LoginErr
                {
                    Message = "Invalid Username and/or Password"
                };
            
            if (l.Attempts > 3)
                return new LoginErr
                {
                    Message = "Max Attempts"
                };

            if (string.IsNullOrEmpty(l.Orgs))
                return new LoginErr
                {
                    Message = "Invalid Organisation"
                };
            
            List<int> numbers = l.Orgs
                .Split(',')
                .Select(int.Parse)
            .ToList();

            if (!numbers.Contains(org))
                return new LoginErr
                {
                    Message = "Invalid Organisation"
                };

            return null;
        }


        public async Task<UserEnt> InitialiseLogin(LoginEnt l, OrgEnt org)
        {
            await SetAttempts(l.Id, 0);
            var permissions = await _permissionService.LoadPermissions (l.Id, org.Id);

            var user = new UserEnt
            {
                LoginId = l.Id,
                Userid = l.Userid,
                Permissions = permissions,
            };

            return user;
        }

        public async Task<bool> IncrementAttempts(LoginEnt l)
        {
            return await SetAttempts(l.Id, ++l.Attempts);
        }

        private async Task<bool> SetAttempts(int id, int attempts)
        {
            await Sql.Execute(
                   "UPDATE base.zzz "
                   + "SET Attempts = " + attempts + " "
                   + "WHERE id = " + id
               );
            return true;
        }

    }
}
