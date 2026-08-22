using Bogus;
using PhoenixKC.Data.Features.Auth.RefreshTokens;

namespace PhoenixKC.WebAPI.Features.Auth.Dtos.RefreshTokens;

public static class RefreshTokenEntityFaker
{
    extension(Faker<RefreshTokenEntity> thisFaker)
    {
        public Faker<RefreshTokenEntity> ValidInstance()
        {
            return thisFaker.CustomInstantiator(g =>
            {
                return new RefreshTokenEntity()
                {
                    Value = g.Random.String2(RefreshTokenEntityConstants.RefreshTokenMaxLength),
                    ExpiresAt = DateTime.UtcNow.AddDays(1)
                };
            });
        }
        public Faker<RefreshTokenEntity> WithId(Guid id)
        {
            return thisFaker.RuleFor(e => e.Id, g => id);
        }
        public Faker<RefreshTokenEntity> WithUserId(Guid userId)
        {
            return thisFaker.RuleFor(e => e.UserId, g => userId);
        }
        public Faker<RefreshTokenEntity> MakeExpired()
        {
            return thisFaker.RuleFor(
                e => e.ExpiresAt,
                g => DateTime.UtcNow.AddDays(-1)
            );
        }
    }
}