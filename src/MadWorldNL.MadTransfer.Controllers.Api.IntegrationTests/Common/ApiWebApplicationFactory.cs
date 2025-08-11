using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace MadWorldNL.MadTransfer.Common;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string DbUserName = "TestDbUser";
    private const string DbPassword = "TestDbPassword";
    
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder()
            .WithUsername(DbUserName)
            .WithPassword(DbPassword)
            .Build();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _postgresContainer.StartAsync().GetAwaiter().GetResult();
        
        builder.ConfigureHostConfiguration(config =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["Authentication:ValidateUser"] = "false",
                ["DatabaseSettings:Host"] = _postgresContainer.Hostname,
                ["DatabaseSettings:Port"] = _postgresContainer.GetMappedPublicPort(5432).ToString(),
                ["DatabaseSettings:User"] = DbUserName,
                ["DatabaseSettings:Password"] = DbPassword
            };

            config.AddInMemoryCollection(testSettings);
        });
        return base.CreateHost(builder);
    }

    /// <summary>
    /// Returns a hardcoded JWT token intended **only for integration testing** purposes.
    /// 
    /// <b>Generate new one:</b> <br></br>
    /// dotnet user-jwts create -n DonaldDuck
    /// </summary>
    /// <returns>JWT token</returns>
    public string GetJwtToken()
    {
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjNjOTQ0ZDIxLTRkNTYtNDhhMy1iMjc1LTMzNzhlMTFjODI2YiIsInN1YiI6IjNjOTQ0ZDIxLTRkNTYtNDhhMy1iMjc1LTMzNzhlMTFjODI2YiIsImp0aSI6ImRhOGZkNTcwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTI5MyIsImh0dHBzOi8vbG9jYWxob3N0OjcyOTkiXSwibmJmIjoxNzU0OTM2NTY2LCJleHAiOjQ5MDg1MzY1NjYsImlhdCI6MTc1NDkzNjU2NiwiaXNzIjoiZG90bmV0LXVzZXItand0cyJ9.q6jcxWmfWhfYOPVVLKxfq7rnoRoDq0pXUmmQOQSXSuo";
    }

    public override async ValueTask DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await base.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
}