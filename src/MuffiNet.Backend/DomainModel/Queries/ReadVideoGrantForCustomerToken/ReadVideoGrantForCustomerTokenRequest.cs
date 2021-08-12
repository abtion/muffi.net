using MediatR;
using System;

namespace MuffiNet.Backend.DomainModel.Queries.ReadVideoGrantForCustomerToken
{
    public class ReadVideoGrantForCustomerTokenRequest : IRequest<ReadVideoGrantForCustomerTokenResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}