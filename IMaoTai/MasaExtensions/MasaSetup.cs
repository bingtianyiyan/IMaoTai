using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BlazorComponent.I18n;

namespace IMaoTai.MasaExtensions;

public static class MasaSetup
{
    public static void AddMasaSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddMasaBlazor();
        services.TryAddScoped<I18n>();
        services.TryAddScoped<CookieStorage>();
        services.AddHttpContextAccessor();
    }
}