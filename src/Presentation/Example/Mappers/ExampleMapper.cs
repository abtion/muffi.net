using Domain.Example.Entities;
using Presentation.Example.Dtos;
using Presentation.Shared;

namespace Presentation.Example.Mappers;

public class ExampleMapper
{
    public ExampleDto MapEntityToDto(ExampleEntity entity)
    {
        return new ExampleDto(entity.Id, entity.Name, entity.Description, entity.Email, entity.Phone);
    }

    public ExampleEntity MapDtoToEntity(ExampleDto dto)
    {
        return new ExampleEntity() { Id = dto.Id, Name = dto.Name, Description = dto.Description, Email = dto.Email, Phone = dto.Phone };
    }

    public ExampleEntity MapCreateCommandToEntity(ExampleCreateCommand dto)
    {
        return new ExampleEntity() { Name = dto.Name, Description = dto.Description, Email = dto.Email, Phone = dto.Phone };
    }

    internal void MapUpdateRequestToEntity(ExampleUpdateCommand request, ExampleEntity entity)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
    }
}
