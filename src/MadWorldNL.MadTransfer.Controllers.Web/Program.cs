using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Configurations;
using MadWorldNL.MadTransfer.Security;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var apiSettingsSection = builder
    .Configuration
    .GetRequiredSection(ApiSettings.Key);
var apiSettings = apiSettingsSection.Get<ApiSettings>()!;

builder.Services.Configure<ApiSettings>(apiSettingsSection);
builder.Services.AddScoped<MyAuthorizationMessageHandler>();
builder.Services.AddHttpClient(ApiNames.Anonymous, client => client.BaseAddress = new Uri(apiSettings.Url));
builder.Services.AddHttpClient(ApiNames.Authorization, client => client.BaseAddress = new Uri(apiSettings.Url))
    .AddHttpMessageHandler<MyAuthorizationMessageHandler>();

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();