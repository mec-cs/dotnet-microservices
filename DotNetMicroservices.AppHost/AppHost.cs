var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Locations_API>("locations-api");

builder.AddProject<Projects.Users_API>("users-api");

builder.Build().Run();