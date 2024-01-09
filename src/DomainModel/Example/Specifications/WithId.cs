using Ardalis.Specification;
using Domain.Example.Entities;

namespace Domain.Example.Specifications;

public class WithId : Specification<ExampleEntity>
{
    public WithId(int id)
    {
        Query.Where(example => example.Id == id);
    }
}