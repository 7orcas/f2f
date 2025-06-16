using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using GC = Backend.GlobalConstants;

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
        private readonly TokenServiceI _tokenService;
        private readonly OrgServiceI _orgService;
        private readonly ConfigServiceI _configService;
        private readonly SessionServiceI _sessionService;

        public LoginService (IServiceProvider serviceProvider,
            TokenServiceI tokenService,
            OrgServiceI orgService,
            ConfigServiceI configService,
            PermissionServiceI permissionService,
            SessionServiceI sessionService) 
            : base (serviceProvider)
        {
            _tokenService = tokenService;
            _orgService = orgService;
            _configService = configService;
            _permissionService = permissionService;
            _sessionService = sessionService;
        }

        public async Task<LoginEnt> LoginUser(string userid, string password, int orgNr, int sourceAppNr, string? langCode)
        {
            try
            {
                var login = await GetLogin(userid);
                if (!login.Response.Valid) return login;

                var err = await Validate(login, password, orgNr);
                if (err != null)
                {
                    login.Response.Valid = false;
                    login.Response.ErrorMessage = err;
                    return login;
                }

                langCode = !string.IsNullOrEmpty(langCode) ? langCode : login.LangCode;
                var org = await _orgService.GetOrg(orgNr);
                var user = await InitialiseLogin(login, org, sourceAppNr);
                var config = _configService.CreateUserConfig(user, org, langCode);
                var session = await _sessionService.CreateSession(user, org, config, sourceAppNr);

                var tv = new TokenValues
                {
                    Username = userid,
                    SessionKey = session.Key,
                    Org = orgNr,
                };

                var tokenX = _tokenService.CreateToken(tv);
                var keyX = Guid.NewGuid().ToString();
                _tokenService.AddToken(keyX, tokenX);


                login.Response.Token = tokenX;
                login.Response.TokenKey = keyX;
                login.Response.MainUrl = AppSettings.MainClientUrl;
                login.Response.LangCode = config.LangCodeCurrent;

                return login;
            }
            catch 
            {
                var login = new LoginEnt();
                login.Response.Valid = false;
                login.Response.ErrorMessage = "Unknown Error";
                return login;
            }
        }

        public async Task<LoginEnt> GetLogin(string userid)
        {
            var l = new LoginEnt();
            l.Response.Valid = false;

            try
            {
                //ToDo Log
                if (!ValidateParameter(userid))
                    return l;

                await Sql.Run(
                    "SELECT * FROM base.zzz "
                    + "WHERE xxx = @userid ",
                    r =>
                    {
                        l = new LoginEnt { 
                            Id = GetId(r, "id"),
                            Userid = GetString(r, "xxx"),
                            Password = GetString(r, "yyy"),
                            Orgs = GetString(r, "orgs"),
                            LangCode = GetStringNull(r, "langCode"),
                            Attempts = GetIntNull(r, "attempts"),
                            Lastlogin = GetDateTime(r, "lastlogin"),
                            IsActive = GetBoolean(r, "isActive")
                        };
                        l.Response.Valid = true;
                    },
                    new SqlParameter("@userid", userid)
                );
            }
            catch { }
            return l;
        }

        //ToDo Language codes!
        private async Task<string> Validate (LoginEnt l, string password, int org)
        {
            if (l == null || l.Id == 0)
                return "Invalid Username and/or Password.";

            await IncrementAttempts(l);

            if (string.IsNullOrEmpty(password) || !password.Equals(l.Password))
                return "Invalid Username and/or Password";
            
            if (l.Attempts > 3)
                return "Max Attempts";

            if (string.IsNullOrEmpty(l.Orgs))
                return  "Invalid Organisation";
            
            List<int> numbers = l.Orgs
                .Split(',')
                .Select(int.Parse)
            .ToList();

            if (!numbers.Contains(org))
                return "Invalid Organisation";

            return null;
        }


        public async Task<UserEnt> InitialiseLogin(LoginEnt l, OrgEnt org, int sourceAppNr)
        {
            await SetAttempts(l.Id, 0);
            var permissions = await _permissionService.LoadEffectivePermissionsInt(l.Id, org.Id);

            var user = new UserEnt
            {
                LoginId = l.Id,
                Userid = l.Userid,
                Permissions = permissions,
            };

            _auditService.LogInOut(sourceAppNr, org.Id, l.Id, GC.EntityTypeLogin);
            return user;
        }

        private async Task<bool> IncrementAttempts(LoginEnt l)
        {
            if (l.Attempts == null)
                l.Attempts = 0;
            l.Attempts += 1;
            return await SetAttempts(l.Id, l.Attempts.Value);
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
