using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.EstimatedWaitingTime;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadVideoGrantForCustomerToken;

namespace MuffiNet.FrontendReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateSupportTicketResponse>> CreateSupportTicket([FromServices] CreateSupportTicketHandler handler, CreateSupportTicketRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }

        [HttpGet("roomgranttoken")]
        public async Task<ActionResult<ReadVideoGrantForCustomerTokenResponse>> ReadCustomerRoomGrantToken([FromServices] ReadVideoGrantForCustomerTokenHandler handler, string supportTicketId, CancellationToken cancellationToken)
        {
            var request = new ReadVideoGrantForCustomerTokenRequest() { SupportTicketId = new Guid(supportTicketId) };

            return await handler.Handle(request, cancellationToken);
        }

        [HttpGet("estimatedwaitingtime")]
        public async Task<ActionResult<EstimatedWaitingTimeResponse>> EstimatedWaitingTime([FromServices] EstimatedWaitingTimeHandler handler, string supportTicketId, CancellationToken cancellationToken)
        {
            var request = new EstimatedWaitingTimeRequest() { SupportTicketId = new Guid(supportTicketId) };

            return await handler.Handle(request, cancellationToken);
        }
    }
}