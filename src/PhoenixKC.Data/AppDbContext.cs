using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Features.Auth.Users;
using PhoenixKC.Data.Features.Auth.Roles;
using PhoenixKC.Data.Shared.KeyedEntities;
using PhoenixKC.Data.Features.Auth.RefreshTokens;
using PhoenixKC.Data.Shared.CreationTimeEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PhoenixKC.Data;

public sealed class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    #region Instance
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!; //Init by EFCore

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    #endregion

    #region Base
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.GenerateIdForKeyedEntities();
        this.SetUtcNowForCreationTimeEntities();
        int changedNumber = base.SaveChanges(acceptAllChangesOnSuccess);
        base.ChangeTracker.Clear();
        return changedNumber;
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.GenerateIdForKeyedEntities();
        this.SetUtcNowForCreationTimeEntities();
        int changedNumber = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        base.ChangeTracker.Clear();
        return changedNumber;
    }
    #endregion
}