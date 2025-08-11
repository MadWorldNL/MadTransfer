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
                ["DatabaseSettings:Host"] = _postgresContainer.Hostname,
                ["DatabaseSettings:Port"] = _postgresContainer.GetMappedPublicPort(5432).ToString(),
                ["DatabaseSettings:User"] = DbUserName,
                ["DatabaseSettings:Password"] = DbPassword
            };

            config.AddInMemoryCollection(testSettings);
        });
        return base.CreateHost(builder);
    }

    public override async ValueTask DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await base.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
}