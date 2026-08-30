using PhoenixKC.Data.Shared.JoinEntities;
using PhoenixKC.Data.Shared.KeyedEntities;
using PhoenixKC.WebAPI.Shared.Behaviors.Authorized;
using PhoenixKC.Data.Features.Auth.Users.ForeignKey;
using PhoenixKC.WebAPI.Shared.Behaviors.DbTransaction;

namespace PhoenixKC.WebAPI.Shared.JoinEntities;

public interface IUpdateJoinEntitiesMessage<TJoinEntity, TLeftEntity, TRightEntity> : IDbTransactionBehaviorMessage, IAuthorizedBehaviorMessage
    where TJoinEntity : class, IJoinEntity<TJoinEntity, TLeftEntity, TRightEntity>
    where TLeftEntity : class, IKeyedEntity, IUserEntityForeignKey
    where TRightEntity : class, IKeyedEntity, IUserEntityForeignKey
{
    public IReadOnlyCollection<TJoinEntity> OldEntities { get; }
    public IReadOnlyCollection<TJoinEntity> NewEntities { get; }
}