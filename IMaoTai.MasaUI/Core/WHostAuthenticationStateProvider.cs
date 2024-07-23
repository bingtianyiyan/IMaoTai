using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IMaoTai.Core.Domain;
using IMaoTai.Core.Service;
using JsonFlatFileDataStore;
using IMaoTai.Core;

namespace IMaoTai.MasaUI.Core
{
    public class WHostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILoginUserService _loginUserService;

        private ClaimsIdentity identity = new ClaimsIdentity();

        public WHostAuthenticationStateProvider(
            ILoginUserService loginUserService)
        {
            _loginUserService = loginUserService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Open database (create new if file doesn't exist)
            var store = new DataStore(CommonX.LoginCacheUserListFile);
            // Get employee collection
            var collection = store.GetCollection<UserLogin>();

            // Find item with name
            var userSession = collection
                                .AsQueryable()
                                .FirstOrDefault();

            if (userSession != null)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, userSession.UserName)};
                identity = new ClaimsIdentity(claims, "IMaoTai");
            }
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult( new AuthenticationState(user));

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
                var claims = new[] { new Claim(ClaimTypes.Name, loginParameters.UserName) };
                identity = new ClaimsIdentity(claims, "IMaoTai");

                // Open database (create new if file doesn't exist)
                var store = new DataStore(CommonX.LoginCacheUserListFile);
                // Get employee collection
                var collection = store.GetCollection<UserLogin>();

                // Find item with name
                var userDynamic = collection
                                    .AsQueryable()
                                    .FirstOrDefault(p => p.UserName == loginParameters.UserName);
                if (userDynamic == null)
                {
                    await collection.InsertOneAsync(loginParameters);
                }
                else
                {
                    await collection.UpdateOneAsync(p => p.UserName == loginParameters.UserName, loginParameters);
                }


                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return result;
        }
    }
}