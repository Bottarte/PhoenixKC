using PhoenixKC.AppHost;
using Aspire.Hosting.JavaScript;
using Arshid.Aspire.ApiDocs.Extensions;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<SqlServerServerResource> sqlserver = builder.AddSqlServer(AppHostConstants.SqlServer);
IResourceBuilder<SqlServerDatabaseResource> database = sqlserver.AddDatabase(AppHostConstants.Database);
IResourceBuilder<ProjectResource> webapi = builder.AddProject<Projects.PhoenixKC_WebAPI>(AppHostConstants.WebAPI);
IResourceBuilder<ViteAppResource> ui = builder.AddViteApp(AppHostConstants.UI, "../PhoenixKC.UI", "start");

webapi.WithReference(database).WaitFor(database).WithScalar(true).WithOpenApi(true).WithEnvironment(AppHostConstants.UIOrigin, ui.GetEndpoint("http"));
ui.WithReference(webapi).WaitFor(webapi).WithHttpEndpoint(env: "PORT").WithExternalHttpEndpoints();
await builder.Build().RunAsync();