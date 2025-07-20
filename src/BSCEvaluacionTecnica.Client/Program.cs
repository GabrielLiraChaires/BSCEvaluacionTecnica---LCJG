using Blazored.SessionStorage;
using BSCEvaluacionTecnica.Business.Interfaces;
using BSCEvaluacionTecnica.Business.Services;
using BSCEvaluacionTecnica.Client;
using BSCEvaluacionTecnica.Shared.Extensions;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//API.
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5224/") }
);

//Agregar servicios de autenticación.
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, AutenticacionExtension>();
builder.Services.AddAuthorizationCore();

//IAccesoService.
builder.Services.AddScoped<IAccesoService, AccesoService>();
//IPermisoModulosService.
builder.Services.AddScoped<IPermisoModulosService, PermisoModulosService>();
//IPermisoModulosService.
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

//SweetAlert.
builder.Services.AddSweetAlert2();

// Configura OIDC usando metadata local para evitar CORS.
builder.Services.AddOidcAuthentication(options =>
{
    //Carga ClientId (y Authority, aunque no se use) desde config.
    builder.Configuration.Bind("Local", options.ProviderOptions);

    //Fichero estático.
    options.ProviderOptions.MetadataUrl =
        $"{builder.HostEnvironment.BaseAddress}oidc-metadata.json";

    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
});

await builder.Build().RunAsync();
