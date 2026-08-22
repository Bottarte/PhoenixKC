namespace PhoenixKC.Data.Shared.CreationTimeEntities;

public interface ICreationTimeEntity
{
    public abstract DateTime CreatedAt { get; set; }
}