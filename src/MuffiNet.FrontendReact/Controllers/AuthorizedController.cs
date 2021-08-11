using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicketById;
using MuffiNet.FrontendReact.Exceptions;

namespace MuffiNet.FrontendReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorizedController : ControllerBase
    {
        [HttpGet("allsupporttickets")]
        public async Task<ActionResult<ReadSupportTicketResponse>> ReadSupportTickets([FromServices] ReadSupportTicketHandler handler, CancellationToken cancellationToken, bool includeCompletedSupportTickets = false)
        {
            return await handler.Handle(new ReadSupportTicketRequest() { IncludeCompletedSupportTickets = includeCompletedSupportTickets }, cancellationToken);
        }

        [HttpDelete]
        public async Task<ActionResult<DeleteSupportTicketResponse>> DeleteSupportTicket([FromServices] DeleteSupportTicketHandler handler, DeleteSupportTicketRequest request, CancellationToken cancellationToken)
        {
            return await handler.Handle(request, cancellationToken);
        }


        [HttpGet("supportticket")]
        public async Task<ActionResult<ReadSupportTicketByIdResponse>> ReadSupportTicketById([FromServices] ReadSupportTicketByIdHandler handler, string supportTicketId, CancellationToken cancellationToken)
        {
            var request = new ReadSupportTicketByIdRequest();

            if (Guid.TryParse(supportTicketId, out Guid id))
                request.SupportTicketId = id;
            else
                throw new SupportTicketIdInvalidException();

            return await handler.Handle(request, cancellationToken);
        }


        [HttpGet("requestossidfromoss")]
        public async Task<ActionResult<RequestOssIdFromOssResponse>> RequestOssIdFromOss([FromServices] RequestOssIdFromOssHandler handler, string supportTicketId, CancellationToken cancellationToken)
        {
            var request = new RequestOssIdFromOssRequest();
            request.SupportTicketId = supportTicketId;

            return await handler.Handle(request, cancellationToken);
        }
    }
}