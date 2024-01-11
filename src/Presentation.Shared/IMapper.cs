namespace Presentation.Shared;

public interface IMapper<E, D> where E : class where D : class
{
    public D MapFromEntity(E entity);
    public E MapToEntity(D dto);
}
