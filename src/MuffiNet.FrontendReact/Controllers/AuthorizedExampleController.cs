using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AuthorizedExampleController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ExampleQueryResponse>> ExampleQuery([FromServices] ExampleQueryHandler handler, [FromQuery] int idOfExampleEntity, CancellationToken cancellationToken)
        {
            return await handler.Handle(new ExampleQueryRequest() { Id = idOfExampleEntity }, cancellationToken);
        }

        // endpoints with complex names must use all lower case and hyphens to separate words
        [HttpGet("get-all")]
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