using IMaoTai.MasaBlazor.Components;
using IMaoTai.MasaBlazor.Web.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

IocHelper.RegisterService(builder.Services);

builder.Services.AddMasaBlazor();

await IocHelper.InitBusiness();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
