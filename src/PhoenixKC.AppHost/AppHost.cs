using PhoenixKC.AppHost;
using Aspire.Hosting.JavaScript;
using Arshid.Aspire.ApiDocs.Extensions;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<SqlServerServerResource> sqlserver = builder.AddSqlServer(AppHostResources.SqlServer);
IResourceBuilder<SqlServerDatabaseResource> database = sqlserver.AddDatabase(AppHostResources.AppDatabase);
IResourceBuilder<ProjectResource> web = builder.AddProject<Projects.PhoenixKC_WebAPI>(AppHostResources.WebAPI);
IResourceBuilder<ViteAppResource> ui = builder.AddViteApp(AppHostResources.WebUI, "../PhoenixKC.WebUI", "start");

web.WithReference(database).WaitFor(database).WithScalar(true).WithOpenApi(true).WithEnvironment(AppHostConstants.UIOrigin, ui.GetEndpoint("http"));
ui.WithReference(web).WaitFor(web).WithHttpEndpoint(env: "PORT").WithExternalHttpEndpoints();
await builder.Build().RunAsync();