using System;
using System.Threading.Tasks;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;

namespace MuffiNet.Test.Shared.TestData
{
    public class ExampleTestData
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public ExampleTestData(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task AddExampleEntitiesToDatabase(int numberOfEntitiesToAdd)
        {
            for (int i = 0; i < numberOfEntitiesToAdd; i++)
            {
                domainModelTransaction.AddExampleEntity(new ExampleEntity()
                {
                    Id = i + 1,
                    Name = $"Name {i}",
                    Description = $"Description {i}"
                });
            }
            
            await domainModelTransaction.SaveChangesAsync();
        }
    }
}