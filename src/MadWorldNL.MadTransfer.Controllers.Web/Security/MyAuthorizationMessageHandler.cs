using MadWorldNL.MadTransfer.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MadTransfer.Security;

public class MyAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public MyAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IOptions<ApiSettings> apiSettings) : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: [ apiSettings.Value.Url ]
            );
    }
}