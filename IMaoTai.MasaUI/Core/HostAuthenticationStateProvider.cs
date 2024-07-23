﻿using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IMaoTai.Core.Domain;
using IMaoTai.Core.Service;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace IMaoTai.MasaUI.Core
{
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILoginUserService _loginUserService;
        private ProtectedSessionStorage _protectedSessionStore;

        private ClaimsIdentity identity = new ClaimsIdentity();

        public HostAuthenticationStateProvider(
            ILoginUserService loginUserService,
            ProtectedSessionStorage protectedSessionStore)
        {
            _loginUserService = loginUserService;
            _protectedSessionStore = protectedSessionStore;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userSessionStorageResult = await _protectedSessionStore.GetAsync<UserLogin>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
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
                await _protectedSessionStore.SetAsync("UserSession", loginParameters);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return result;
        }
    }
}