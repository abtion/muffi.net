using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuffiNet.Backend.DomainModel.Commands.ExampleCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace MuffiNet.FrontendReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ExampleQueryResponse>> ExampleQuery([FromServices] ExampleQueryHandler handler, [FromQuery] int exampleEntityId, CancellationToken cancellationToken)
        {
            return await handler.Handle(new ExampleQueryRequest() { Id = exampleEntityId }, cancellationToken);
        }

        [HttpGet]
        public async Task<ActionResult<ExampleQueryAllResponse>> ExampleQueryAll([FromServices] ExampleQueryAllHandler handler, CancellationToken cancellationToken)
        {
            return await handler.Handle(cancellationToken);
        }

        [HttpPut]
        public async Task<ActionResult<ExampleCommandResponse>> ExampleCommand([FromServices] ExampleCommandHandler handler, ExampleCommandRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }
    }
}