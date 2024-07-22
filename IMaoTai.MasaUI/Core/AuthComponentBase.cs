using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.MasaUI.Core
{
    public class AuthComponentBase : ComponentBase
    {
        // 注入相关服务
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ClaimsPrincipal User { get; set; }


        // 初始化时获取认证状态
        protected override async Task OnInitializedAsync()
        {
            await GetAuthenticationStateAsync();
        }


        // 检查用户的认证状态，未认证则重定向至登录页面
        private async Task GetAuthenticationStateAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            User = authenticationState.User;


            if (!User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}
