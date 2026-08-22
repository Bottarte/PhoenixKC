namespace PhoenixKC.Data.Features.Auth.Users.ForeignKey;

public interface IUserEntityForeignKey
{
    public abstract Guid UserId { get; set; }

    public abstract UserEntity? User { get; set; }
}