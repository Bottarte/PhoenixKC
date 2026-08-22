using Xunit.v3;
using Xunit.Sdk;
using PhoenixKC.IntegrationTests;

[assembly: Parallelization(Mode = ParallelMode.None)]
[assembly: AssemblyFixture(typeof(AppFixture))]