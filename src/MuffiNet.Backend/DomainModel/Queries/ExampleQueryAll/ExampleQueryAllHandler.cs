using MediatR;
using System;
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
            return new ExampleQueryAllResponse();
        }
    }
}