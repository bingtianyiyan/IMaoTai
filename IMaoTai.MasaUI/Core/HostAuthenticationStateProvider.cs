// Copyright (c) BeiYinZhiNian (1031622947@qq.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: http://www.caviar.wang/ or https://gitee.com/Cherryblossoms/caviar.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using IMaoTai.Core.Domain;
using IMaoTai.Core.Service;

namespace IMaoTai.MasaUI.Core
{
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILoginUserService _loginUserService;

        //private CurrentUser _currentUser;
        private ClaimsIdentity identity = new ClaimsIdentity();

        public HostAuthenticationStateProvider(ILoginUserService loginUserService)
        {
            _loginUserService = loginUserService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(new ClaimsPrincipal(identity));
            //var identity = new ClaimsIdentity();
            //try
            //{
            //    var userInfo = await GetCurrentUser();
            //    if (userInfo.IsAuthenticated)
            //    {
            //        var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) };

            //        identity = new ClaimsIdentity(claims, nameof(HostAuthenticationStateProvider));
            //    }
            //}
            //catch (HttpRequestException ex)
            //{
            //    Console.WriteLine("Request failed:" + ex.ToString());
            //}
            //return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        //public async Task<CurrentUser> GetCurrentUser()
        //{
        //    if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
        //    _currentUser = await _loginUserService.CurrentUserInfo();
        //    //_currentUser = new CurrentUser()
        //    //{
        //    //    UserName = "admin",
        //    //    IsAuthenticated = true,
        //    //};
        //    return _currentUser;
        //}

        public async Task<string> Logout()
        {
            var result = await _loginUserService.Logout();
            // _currentUser = null;
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
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return result;
        }
    }
}