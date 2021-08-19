using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;
using MuffiNet.Backend.HubContracts;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand
{
    public class ExampleCreateCommandHandler : IRequestHandler<ExampleCreateCommandRequest, ExampleCreateCommandResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly IExampleHubContract exampleHub;

        public ExampleCreateCommandHandler(DomainModelTransaction domainModelTransaction, IExampleHubContract exampleHub)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.exampleHub = exampleHub ?? throw new ArgumentNullException(nameof(exampleHub));
        }

        public async Task<ExampleCreateCommandResponse> Handle(ExampleCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new ExampleEntity();

            // setting the Id since there is no database to do it
            if (domainModelTransaction.ExampleEntities().Any())
                entity.Id = domainModelTransaction.ExampleEntities().Max(p => p.Id) + 1;
            else
                entity.Id = 1;

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Email = request.Email;
            entity.Phone = request.Phone;

            domainModelTransaction.AddExampleEntity(entity);

            await domainModelTransaction.SaveChangesAsync();

            await exampleHub.SomeEntityCreated(new SomeEntityCreatedMessage(entity));

            return new ExampleCreateCommandResponse() { ExampleEntity = entity };
        }
    }
}