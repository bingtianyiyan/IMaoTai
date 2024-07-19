using IMaoTai.MasaBlazor.Components;
using IMaoTai.MasaBlazor.Web.Helpers;
using IMaoTai.MasaUI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

IocHelper.RegisterService(builder.Services);

builder.Services.AddMasaBlazor();

await IocHelper.InitBusiness();
builder.Services.AddAdminCaviar(new Type[] { typeof(Program), typeof(IMaoTai.MasaUI._Imports) });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
    typeof(IMaoTai.MasaUI._Imports).Assembly);

app.Run();
