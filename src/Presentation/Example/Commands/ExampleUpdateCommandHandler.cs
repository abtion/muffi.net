using Domain.Example.Entities;
using Domain.Example.Specifications;
using Domain.Example.UseCases;
using Presentation.Example.Dtos;
using Presentation.Example.Mappers;

namespace Presentation.Example.Commands;

public class ExampleUpdateCommandHandler(UpdateExampleUseCase useCase, IRepository<ExampleEntity> Repository, ExampleMapper Mapper) : ICommandHandler<ExampleUpdateCommand, ExampleUpdateResponse>
{
    public async Task<ExampleUpdateResponse> Handle(ExampleUpdateCommand request, CancellationToken cancellationToken)
    {
        var query = await Repository.GetAll(new WithId(request.Id), cancellationToken);
        var entity = query.SingleOrDefault() ?? throw new EntityNotFoundException(request.Id);

        Mapper.MapUpdateRequestToEntity(request, entity);

        var result = await useCase.Handle(entity, cancellationToken);
        
        var dto = Mapper.MapEntityToDto(result);

        return new ExampleUpdateResponse(dto);
    }
}