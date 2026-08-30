namespace PhoenixKC.Data.Shared.CreationTimeEntities;

public interface ICreationTimeEntity
{
    //Value properties
    public abstract DateTime CreatedAt { get; set; }
}