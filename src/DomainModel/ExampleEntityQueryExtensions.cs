using Domain.Example.Entities;

namespace Domain;

public static class ExampleEntityQueryExtensions
{
    public static IQueryable<ExampleEntity> WithId(this IQueryable<ExampleEntity> entities, int exampleEntityId)
    {
        return entities.Where(p => p.Id == exampleEntityId);
    }

    public static IQueryable<ExampleEntity> All(this IQueryable<ExampleEntity> entities)
    {
        return entities;
    }
}