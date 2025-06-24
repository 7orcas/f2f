using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using GC = Backend.GlobalConstants;

/// <summary>
/// Manage login process for user
/// Note a user can have multiple sessions open
/// Created: April 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Login
{
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

        public async Task<LoginEnt> LoginUser(string userid, string password, int orgId, int sourceAppNr, string? langCode)
        {
            try
            {
                var result = await GetLogin(userid, orgId);
                var login = result.Login;
                if (login == null) 
                    login = new LoginEnt();
                var account = result.Account;

                if (!login.IsActive ||
                    account == null ||
                    !account.IsActive)
                {
                    return login;
                }

                var org = await _orgService.GetOrg(orgId);
                var err = await Validate(login, password, org);
                if (err != null)
                {
                    login.Response.Valid = false;
                    login.Response.ErrorMessage = err;
                    return login;
                }

                langCode = !string.IsNullOrEmpty(langCode) ? langCode : account.LangCode;
                var user = await InitialiseLogin(login, account, org, sourceAppNr);
                var config = _configService.CreateUserConfig(user, org, langCode);
                var session = await _sessionService.CreateSession(user, org, config, sourceAppNr);

                var tv = new TokenValues
                {
                    Username = userid,
                    SessionKey = session.Key,
                    Org = orgId,
                };

                var tokenX = _tokenService.CreateToken(tv);
                var keyX = Guid.NewGuid().ToString();
                _tokenService.AddToken(keyX, tokenX);


                login.Response.Valid = true;
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

        //Each login to an org requires an account record
        //Users can have multiple accounts
        public async Task<(LoginEnt Login, UserAccountEnt Account)> GetLogin(string userid, int orgId)
        {
            var login = null as LoginEnt;
            var account = null as UserAccountEnt;

            try
            {
                //ToDo Log
                if (!ValidateParameter(userid))
                    throw new Exception();

                await Sql.Run(
                    "SELECT * FROM base.zzz " +
                        "WHERE xxx = @userid ",
                    r =>
                    {
                        login = new LoginEnt { 
                            Id = GetId(r),
                            Userid = GetString(r, "xxx"),
                            Password = GetString(r, "yyy"),
                            Attempts = GetIntNull(r, "attempts"),
                            Lastlogin = GetDateTime(r, "lastlogin"),
                            IsActive = GetBoolean(r, "isActive")
                        };
                    },
                    new SqlParameter("@userid", userid)
                );

                await Sql.Run(
                    "SELECT * FROM base.userAcc " +
                        "WHERE zzzId = @zzzId " +
                        "AND orgId = @orgId",
                    r =>
                    {
                        account = new UserAccountEnt
                        {
                            Id = GetId(r),
                            UserId = GetId(r, "zzzId"),
                            OrgId = GetOrgId(r),
                            LangCode = GetStringNull(r, "langCode"),
                            Lastlogin = GetDateTime(r, "lastlogin"),
                            IsActive = IsActive(r),
                            IsAdmin = GetBoolean(r, "isAdmin"),
                            Classification = GetIntNull(r, "classification")
                        };
                    },
                    new SqlParameter("@zzzId", login.Id),
                    new SqlParameter("@orgId", orgId)
                );
            }
            catch { }

            return (login, account);
        }

        //ToDo Language codes!
        private async Task<string> Validate (LoginEnt l, string password, OrgEnt org)
        {
            if (l == null || l.Id == 0)
                return "Invalid Username and/or Password.";

            await IncrementAttempts(l);

            if (string.IsNullOrEmpty(password) || !password.Equals(l.Password))
                return "Invalid Username and/or Password";
            
            if (l.Attempts > org.Encoding.MaxNumberLoginAttempts)
                return "Max Attempts";

            if (!l.IsActive)
                return "In active Login";

            return null;
        }


        public async Task<UserEnt> InitialiseLogin(LoginEnt login, UserAccountEnt account, OrgEnt org, int sourceAppNr)
        {
            await SetAttempts(login.Id, 0);
            var permissions = await _permissionService.LoadEffectivePermissionsInt(account.Id, org.Id);

            var user = new UserEnt
            {
                UserAccountId = account.Id,
                Userid = login.Userid,
                Permissions = permissions,
            };

            _auditService.LogInOut(sourceAppNr, org.Id, account.Id, GC.EntityTypeLogin);
            return user;
        }

        private async Task<bool> IncrementAttempts(LoginEnt l)
        {
            if (l.Attempts == null)
                l.Attempts = 0;
            l.Attempts += 1;
            return await SetAttempts(l.Id, l.Attempts.Value);
        }

        private async Task<bool> SetAttempts(long id, int attempts)
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
