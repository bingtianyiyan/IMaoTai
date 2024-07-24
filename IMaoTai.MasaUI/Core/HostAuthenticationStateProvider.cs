using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IMaoTai.Core.Domain;
using IMaoTai.Core.Service;
using IMaoTai.MasaUI.Core.SessionData;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IMaoTai.Core;

namespace IMaoTai.MasaUI.Core
{
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILoginUserService _loginUserService;
        private readonly ISessionDataService _sessionDataService;

        private ClaimsIdentity identity = new ClaimsIdentity();

        public HostAuthenticationStateProvider(
            ILoginUserService loginUserService,
            ISessionDataService sessionDataService)
        {
            _loginUserService = loginUserService;
            _sessionDataService = sessionDataService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userSession = await _sessionDataService.GetSession();
            if (userSession != null)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, userSession.UserName)};
                identity = new ClaimsIdentity(claims,"IMaoTai");
            }
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult( new AuthenticationState(user));


            //var user = new ClaimsPrincipal(identity);
            //return new AuthenticationState(new ClaimsPrincipal(identity));

        }


        public ClaimsPrincipal GetCurrentUser()
        {
            var user = new ClaimsPrincipal(identity);
            return user;
        }

        public async Task<string> Logout()
        {
            var result = await _loginUserService.Logout();
            identity = new ClaimsIdentity();
            await _sessionDataService.RemoveSession();
            if (!Config.IsServer)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return result;
        }

        public async Task<bool> Login(UserLogin loginParameters, string returnUrl)
        {
            var result = await _loginUserService.Login(loginParameters, returnUrl);
            if (result)
            {
                CommonX.LoginUserName = loginParameters.UserName;
                var claims = new[] { new Claim(ClaimTypes.Name, loginParameters.UserName) };
                identity = new ClaimsIdentity(claims, "IMaoTai");
                await _sessionDataService.SetSession(loginParameters);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return result;
        }
    }
}