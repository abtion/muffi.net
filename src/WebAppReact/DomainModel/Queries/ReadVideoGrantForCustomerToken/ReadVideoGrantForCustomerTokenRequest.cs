using MediatR;
using System;

namespace WebAppReact.DomainModel.Queries.ReadVideoGrantForCustomerToken
{
    public class ReadVideoGrantForCustomerTokenRequest : IRequest<ReadVideoGrantForCustomerTokenResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}