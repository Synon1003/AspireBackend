#!/usr/bin/env dotnet
#:package Aspire.Hosting.AppHost@17.0.0
#:package Aspire.Hosting.Azure.Sql@17.0.0
#:package Aspire.Hosting.Dotnet@17.0.0

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddAzureSqlServer("sqlserver")
                       .RunAsContainer(sql =>
                       {
                            sql.WithHostPort(1433);
                            sql.WithLifetime(ContainerLifetime.Persistent);
                           //sql.WithDataVolume();
                       });
var shoppingListDb = sqlServer.AddDatabase("ShoppingListDb");

var blobs = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithLifetime(ContainerLifetime.Persistent);
        azurite.WithBlobPort(10000);
        azurite.WithQueuePort(10001);
        azurite.WithTablePort(10002);
    })
    .AddBlobs("picturestore");

var api = builder.AddDotnetProject("api", "./AspireBackend.Api/")
    .WithReference(shoppingListDb)
    .WaitFor(shoppingListDb)
    .WithReference(blobs)
    .WithEnvironment("SCALAR_CLIENTID", builder.Configuration["Scalar:ClientId"])
    .WithEnvironment("SCALAR_AUTHORIZATIONURL", builder.Configuration["Scalar:AuthorizationUrl"])
    .WithEnvironment("SCALAR_TOKENURL", builder.Configuration["Scalar:TokenUrl"])
    .WithUrls(context =>
    {
        context.Urls.Add(new()
        {
            Url = "/scalar/v1",
            DisplayText = "API Docs",
            Endpoint = context.GetEndpoint("http")
        });
    })
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();