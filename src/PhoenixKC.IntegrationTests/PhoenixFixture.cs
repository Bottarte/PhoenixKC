using Respawn;
using Microsoft.Data.SqlClient;
using PhoenixKC.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace PhoenixKC.IntegrationTests;

public sealed class PhoenixFixture : IAsyncLifetime
{
    #region Instance
    private PhoenixApplication Application { get; } = new();
    private string ConnectionString { get; set; } = null!; //Init after InitializedAsync
    private DbContextOptions<PhoenixDbContext> DbOptions { get; set; } = null!; //Init after InitializedAsync
    private Respawner Respawner { get; set; } = null!; //Init after InitializedAsync
    public HttpClient HttpClient { get; private set; } = null!; //Init after InitializedAsync

    public async ValueTask ExecuteAsync(Func<PhoenixDbContext, ValueTask> func)
    {
        await using PhoenixDbContext context = new(DbOptions);
        await func(context);
    }
    public async ValueTask ResetDatabaseAsync()
    {
        await using SqlConnection connection = new(ConnectionString);
        await connection.OpenAsync();
        await Respawner.ResetAsync(connection);
    }
    #endregion

    #region Interfaces
    public async ValueTask InitializeAsync()
    {
        await Application.StartAsync(TestContext.Current.CancellationToken);
        HttpClient = Application.CreateHttpClient("phoenix-webapi");
        ConnectionString = await Application.GetConnectionString("phoenix-database") ?? throw new NullReferenceException("ConnectionString is null");
        DbOptions = new DbContextOptionsBuilder<PhoenixDbContext>().UseSqlServer(ConnectionString).Options;
        await ExecuteAsync(async db =>
        {
            Console.WriteLine("Applying migrations...");
            await db.Database.MigrateAsync();
            Console.WriteLine("Migrations applied");
        });

        await using SqlConnection connection = new(ConnectionString);
        await connection.OpenAsync();
        Respawner = await Respawner.CreateAsync(connection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }
    public async ValueTask DisposeAsync()
    {
        await ResetDatabaseAsync();
        await Application.DisposeAsync();
    }
    #endregion
}