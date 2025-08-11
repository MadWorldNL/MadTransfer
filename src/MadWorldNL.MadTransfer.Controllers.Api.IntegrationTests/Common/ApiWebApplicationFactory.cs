using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace MadWorldNL.MadTransfer.Common;

/// <summary>
/// Injected by integration tests to create the test server and host.
/// </summary>
[UsedImplicitly]
public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string DbUserName = "TestDbUser";
    private const string DbPassword = "TestDbPassword";
    private const int DbPort = 5432;
    
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithUsername(DbUserName)
            .WithPassword(DbPassword)
            .Build();

    private const int StoragePort = 9000;
    
    private readonly IContainer _s3Ninja = new ContainerBuilder()
        .WithImage("scireum/s3-ninja:8.5.0")
        .WithPortBinding(StoragePort, true)
        .Build();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _postgresContainer.StartAsync().GetAwaiter().GetResult();
        _s3Ninja.StartAsync().GetAwaiter().GetResult();
        
        builder.ConfigureHostConfiguration(config =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["Authentication:ValidateUser"] = "false",
                ["DatabaseSettings:Host"] = _postgresContainer.Hostname,
                ["DatabaseSettings:Port"] = _postgresContainer.GetMappedPublicPort(DbPort).ToString(),
                ["DatabaseSettings:User"] = DbUserName,
                ["DatabaseSettings:Password"] = DbPassword,
                ["StorageSettings:Host"] = $"http://{_s3Ninja.Hostname}:{_s3Ninja.GetMappedPublicPort(StoragePort).ToString()}"
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
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik1hZFRyYW5zZmVyIiwic3ViIjoiTWFkVHJhbnNmZXIiLCJqdGkiOiJkMDdhNTA1OSIsIm5hbWVpZCI6ImFhNTg0OWJhLTE0YzMtNDA0OS1hYzQ0LTgxNTZhYjFhODUzYyIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjUyOTMiLCJodHRwczovL2xvY2FsaG9zdDo3Mjk5Il0sIm5iZiI6MTc1NDk0NjY5OSwiZXhwIjo0OTA4NTQ2Njk5LCJpYXQiOjE3NTQ5NDY3MDAsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.e2QDdMEPH9jbWfWynxFKrU-USK-ZYk3sHTlq-fMX0qs";
    }

    public override async ValueTask DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _s3Ninja.DisposeAsync();
        await base.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
}