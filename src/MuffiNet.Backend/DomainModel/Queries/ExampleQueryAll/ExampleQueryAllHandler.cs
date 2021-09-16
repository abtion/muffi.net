using MediatR;
using MuffiNet.Backend.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var query = from exampleEntity in domainModelTransaction.ExampleEntities().All()
                        select new ExampleEntityRecord(
                            exampleEntity.Id,
                            exampleEntity.Name,
                            exampleEntity.Description,
                            exampleEntity.Email,
                            exampleEntity.Phone
                        );

            return await Task.FromResult(new ExampleQueryAllResponse(query.ToList()));
        }
    }
}