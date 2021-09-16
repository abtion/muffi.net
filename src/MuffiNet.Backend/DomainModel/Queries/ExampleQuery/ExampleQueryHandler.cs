using MediatR;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;
using MuffiNet.Backend.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryHandler : IRequestHandler<ExampleQueryRequest, ExampleQueryResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly IExampleReverseStringService exampleService;

        public ExampleQueryHandler(DomainModelTransaction domainModelTransaction, IExampleReverseStringService exampleService)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.exampleService = exampleService ?? throw new ArgumentNullException(nameof(exampleService));
        }

        public async Task<ExampleQueryResponse> Handle(ExampleQueryRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var query = from exampleEntity in domainModelTransaction.ExampleEntities().WithId(request.Id)
                        select new ExampleEntityRecord(
                            exampleEntity.Id,
                            exampleEntity.Name,
                            exampleService.ReverseString(exampleEntity.Description),
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