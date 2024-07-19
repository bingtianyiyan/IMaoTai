using System.Reflection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace IMaoTai.MasaUI
{
    public static class Config
    {
        /// <summary>
        /// 是否为server模式
        /// </summary>
        public static bool IsServer
        {
            get
            {
                if (OperatingSystem.IsBrowser())
                {
                    return false;
                }
                return true;
            }
        }

        public static bool IsDebug { get; set; } = false;
        public static int MaxPageSize { get; set; } = 9999;
        /// <summary>
        /// 是否处理过iframeMessage
        /// </summary>
        public static bool IsHandleIframeMessage { get; set; }
        /// <summary>
        /// 前端所有Assembly
        /// </summary>
        public static List<Assembly> AdditionalAssemblies;

        public static WebAssemblyHostBuilder AddCavWasm(this WebAssemblyHostBuilder builder)
        {
            //IsServer = false;
            //builder.Services.AddScoped<IAuthService, WasmAuthService>();
            builder.Services.AddOptions();
            //builder.Services.AddAuthorizationCore();
            //builder.Services.AddSingleton<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();
            //builder.Services.AddSingleton<IAuthorizationService, DefaultAuthorizationService>();
            return builder;
        }

        public static IServiceCollection AddAdminCaviar(this IServiceCollection services, Type[] assemblies)
        {
            if (assemblies != null)
            {
                AdditionalAssemblies = new List<Assembly>();
                foreach (var item in assemblies)
                {
                    AdditionalAssemblies.Add(item.Assembly);
                }
            }
            return services;
        }
    }
}
