var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects._2048_Infinity_Merge_App>("app-2048-infinity-merge-app");

builder.Build().Run();
