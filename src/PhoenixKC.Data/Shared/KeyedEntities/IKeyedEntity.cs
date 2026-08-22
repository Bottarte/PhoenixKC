namespace PhoenixKC.Data.Shared.KeyedEntities;

public interface IKeyedEntity
{
    public abstract Guid Id { get; set; }
}