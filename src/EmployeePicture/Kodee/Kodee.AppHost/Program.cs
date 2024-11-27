var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Kodee_ApiService>("apiservice");

builder.AddProject<Projects.Kodee_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
