using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppReact.DomainModel.Commands.CompleteRoom;
using WebAppReact.DomainModel.Commands.CreateRoom;
using WebAppReact.DomainModel.Commands.DeleteSupportTicket;
using WebAppReact.DomainModel.Commands.RequestOssIdFromOss;
using WebAppReact.DomainModel.Queries.ReadSupportTicket;
using WebAppReact.DomainModel.Queries.ReadSupportTicketById;
using WebAppReact.Exceptions;

namespace WebAppReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnicianController : ControllerBase
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

        [HttpPost("createroom")]
        public async Task<ActionResult<CreateRoomResponse>> CreateRoomToken([FromServices] CreateRoomHandler createRoomService, CreateRoomRequest createRoomRequest, CancellationToken cancellationToken)
        {
            return await createRoomService.Handle(createRoomRequest, cancellationToken);
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

        [HttpPost("completeroom")]
        public async Task<ActionResult<CompleteRoomResponse>> CompleteRoom([FromServices] CompleteRoomHandler completeRoomHandler, CompleteRoomRequest completeRoomRequest, CancellationToken cancellationToken)
        {
            return await completeRoomHandler.Handle(completeRoomRequest, cancellationToken);
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