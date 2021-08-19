using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorizedExampleController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ExampleQueryResponse>> ExampleQuery([FromServices] ExampleQueryHandler handler, [FromQuery] int idOfExampleEntity, CancellationToken cancellationToken)
        {
            return await handler.Handle(new ExampleQueryRequest() { Id = idOfExampleEntity }, cancellationToken);
        }

        [HttpPut]
        public async Task<ActionResult<ExampleCreateCommandResponse>> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommandRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }


    }
}