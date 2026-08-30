using Projects;

namespace PhoenixKC.IntegrationTests;

public sealed class AppFactory(params string[] args) : DistributedApplicationFactory(typeof(PhoenixKC_AppHost), args);