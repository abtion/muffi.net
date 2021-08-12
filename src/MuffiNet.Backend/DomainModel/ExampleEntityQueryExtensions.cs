using System;
using System.Linq;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel
{
    public static class ExampleEntityQueryExtensions
    {
        public static IQueryable<ExampleEntity> WithId(this IQueryable<ExampleEntity> entities, int exampleEntityId)
        {
            return entities.Where(p => p.Id == exampleEntityId);
        }
    }
}