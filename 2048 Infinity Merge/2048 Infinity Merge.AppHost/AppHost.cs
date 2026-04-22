var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.InfinityMerge2048_App>("infinitymerge2048-app");

builder.Build().Run();
