using Microsoft.AspNetCore.Mvc;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll;
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
            return await handler.Handle(new ExampleQueryAllRequest(), cancellationToken);
        }

        [HttpPut]
        public async Task<ActionResult<ExampleCreateCommandResponse>> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommandRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<ExampleDeleteCommandResponse>> ExampleDeleteCommand([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommandRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }
    }
}