var builder = DistributedApplication.CreateBuilder(args);

var api1 = builder.AddProject<Projects.POC_Api>("api-example-1");
var api2 = builder.AddProject<Projects.POC_Api2>("api-example-2");
builder.AddProject<Projects.IdentityServerAspNetIdentity>("identity-server");
builder.AddProject<Projects.wasmwithids_Server>("blazor-ui")
    .WithReference(api1)
    .WithReference(api2);
builder.Build().Run();
