IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<SqlServerServerResource> sql = builder.AddSqlServer("phoenix-sql").WithDataVolume();
IResourceBuilder<SqlServerDatabaseResource> database = sql.AddDatabase("phoenix-database");
IResourceBuilder<ProjectResource> webapi = builder.AddProject<Projects.PhoenixKC_WebAPI>("phoenix-webapi")
    .WithReference(database).WaitFor(database)
    .WithExternalHttpEndpoints();
builder.AddViteApp("phoenix-ui", "../../PhoenixKC.UI", "start")
    .WithReference(webapi).WaitFor(webapi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();
builder.Build().Run();