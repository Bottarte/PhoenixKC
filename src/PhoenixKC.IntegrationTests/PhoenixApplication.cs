using Projects;
using Aspire.Hosting.Testing;

namespace PhoenixKC.IntegrationTests;

public sealed class PhoenixApplication(params string[] args) : DistributedApplicationFactory(typeof(PhoenixKC_Deploy_AppHost), args);