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

        public async Task<ExampleEntity> CreateExampleTestData()
        {
            //    var exampleEntity = new ExampleEntity()
            //    {

            //    };

            //    await domainModelTransaction.AddAsync<ExampleEntity>(ExampleEntity);
            //    await domainModelTransaction.SaveChangesAsync();

            //    return exampleEntity;
            throw new NotImplementedException();
        }
    }
}
