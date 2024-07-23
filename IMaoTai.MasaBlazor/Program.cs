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
builder.Services.AddIMaoTaiAdmin(new Type[] { typeof(Program), typeof(IMaoTai.MasaUI._Imports) });

//增加API允许跨域调用
builder.Services.AddCors(options => options.AddPolicy("Any",
    builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseCors("Any");
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
    typeof(IMaoTai.MasaUI._Imports).Assembly);

app.Run();
