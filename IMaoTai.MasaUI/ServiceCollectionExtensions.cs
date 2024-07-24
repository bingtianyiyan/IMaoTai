using IMaoTai.Core.Service;
using IMaoTai.MasaUI.Core.SessionData;
using IMaoTai.MasaUI.Core;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IMaoTai.MasaUI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIMaoTaiBusinessService(this IServiceCollection service)
        {
            service.AddAuthorizationCore();
            service.AddCascadingAuthenticationState();
            service.AddScoped<HostAuthenticationStateProvider>();
            service.AddScoped<AuthenticationStateProvider, HostAuthenticationStateProvider>();
            service.TryAddSingleton<IUserService, UserService>();
            service.TryAddSingleton<IAppointProjectService, AppointProjectService>();
            service.TryAddSingleton<IShopService, ShopService>();
            service.TryAddSingleton<ILogService, LogService>();
            service.TryAddSingleton<ILoginUserService, LoginUserService>();
            return service;
        }
    }
}
