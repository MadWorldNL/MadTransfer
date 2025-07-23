using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace MadWorldNL.MadTransfer.Security;

public class MyAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public MyAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:7299/" });
    }
}