using Xunit.v3;
using Xunit.Sdk;
using PhoenixKC.IntegrationTests;

[assembly: AssemblyFixture(typeof(AppFixture))]
[assembly: Parallelization(Mode = ParallelMode.None)]