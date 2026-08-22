using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Shared.KeyedEntities;
using PhoenixKC.Data.Features.Auth.Users.ForeignKey;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoenixKC.Data.Features.Auth.RefreshTokens;

public sealed class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ConfigureKeyedEntity();
        builder.Property(e => e.Value).IsRequired().HasMaxLength(RefreshTokenEntityConstants.RefreshTokenMaxLength);
        builder.HasIndex(e => e.Value).IsUnique();
        builder.Property(e => e.ExpiresAt).IsRequired();
        builder.ConfigureUserEntityForeignKey(e => e.RefreshTokens);
    }
    #endregion
}