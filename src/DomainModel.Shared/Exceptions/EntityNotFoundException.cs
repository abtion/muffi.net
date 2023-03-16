namespace DomainModel.Shared.Exceptions;

public class EntityNotFoundException : Exception
{

    public EntityNotFoundException(int entityId) : base($"Entity with id {entityId} was not found")
    {
    }
}