using MadWorldNL.MadTransfer;
using MadWorldNL.MadTransfer.Databases;
using MadWorldNL.MadTransfer.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddAntiforgery();

builder.Services.AddDbContextPool<MadTransferContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("MadTransferContext"),
        o => o
            .SetPostgresVersion(17, 0)
            .UseNodaTime()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapHealthChecks("/healthz");
app.AddFileEndpoints();

app.Services.MigrateDatabase<MadTransferContext>();

app.Run();