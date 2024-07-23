var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IMaoTai_MasaBlazor>("imaotai-masablazor");

builder.Build().Run();
