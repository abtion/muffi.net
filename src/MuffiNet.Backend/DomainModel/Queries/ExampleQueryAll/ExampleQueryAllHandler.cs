using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll.ExampleQueryAllResponse;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll
{
    public class ExampleQueryAllHandler : IRequestHandler<ExampleQueryAllRequest, ExampleQueryAllResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public ExampleQueryAllHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<ExampleQueryAllResponse> Handle(ExampleQueryAllRequest request, CancellationToken cancellationToken)
        {
            var query = from exampleEntity in domainModelTransaction.ExampleEntities().All()
                        select new ExampleEntityRecord(
                            exampleEntity.Id,
                            exampleEntity.Name,
                            exampleEntity.Description,
                            exampleEntity.Email,
                            exampleEntity.Phone
                        );

            return await Task.FromResult(new ExampleQueryAllResponse() { ExampleEntities = query.ToList() });
        }
    }
}