using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryHandler : IRequestHandler<ExampleQueryRequest, ExampleQueryResponse>
    {
        private DomainModelTransaction domainModelTransaction;

        public ExampleQueryHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }
        
        public async Task<ExampleQueryResponse> Handle(ExampleQueryRequest request, CancellationToken cancellationToken)
        {
            var query = domainModelTransaction.ExampleEntities().WithId(request.Id);

            if (!query.Any())
                throw new ExampleEntityNotFoundException(request.Id);

            return new ExampleQueryResponse() 
            { 
                ExampleEntity = query.SingleOrDefault()
            };
        }
    }
}