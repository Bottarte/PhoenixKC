using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoenixKC.Infrastructure.Features.Example;

public sealed class ExampleConfiguration : IEntityTypeConfiguration<ExampleEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<ExampleEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(ExampleConstants.TitleMaxLength);
    }
    #endregion
}