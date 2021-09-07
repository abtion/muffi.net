using MediatR;
using MuffiNet.Backend.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MuffiNet.Backend.DomainModel.Queries.ExampleQuery.ExampleQueryResponse;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryHandler : IRequestHandler<ExampleQueryRequest, ExampleQueryResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public ExampleQueryHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<ExampleQueryResponse> Handle(ExampleQueryRequest request, CancellationToken cancellationToken)
        {
            var query = from exampleEntity in domainModelTransaction.ExampleEntities().WithId(request.Id)
                        select new ExampleEntityRecord(
                            exampleEntity.Id,
                            exampleEntity.Name,
                            exampleEntity.Description,
                            exampleEntity.Email,
                            exampleEntity.Phone
                        );

            if (!query.Any())
                throw new ExampleEntityNotFoundException(request.Id);

            return await Task.FromResult(new ExampleQueryResponse()
            {
                ExampleEntity = query.Single()
            });
        }
    }
}