using System;
using System.Linq;
using MuffiNet.FrontendReact.Models;

namespace MuffiNet.FrontendReact.DomainModel
{
    public static class SupportTicketQueryExtensions
    {
        public static IQueryable<SupportTicket> WithSupportTicketId(this IQueryable<SupportTicket> entities, string supportTicketId)
        {
            return entities.WithSupportTicketId(new Guid(supportTicketId));
        }

        public static IQueryable<SupportTicket> WithSupportTicketId(this IQueryable<SupportTicket> entities, Guid supportTicketId)
        {
            return entities.Where(p => p.SupportTicketId == supportTicketId);
        }

        public static IQueryable<SupportTicket> OrderedByCreatedTime(this IQueryable<SupportTicket> entities)
        {
            return entities.OrderBy(p => p.CreatedAt);
        }

        public static IQueryable<SupportTicket> NotCompleted(this IQueryable<SupportTicket> entities)
        {
            return entities.Where(p => p.CallEndedAt == null);
        }

        public static IQueryable<SupportTicket> NotStarted(this IQueryable<SupportTicket> entities)
        {
            return entities.Where(p => p.CallStartedAt == null);
        }

        public static IQueryable<SupportTicket> AddedToQueueBeforeThis(this IQueryable<SupportTicket> entities, DateTime addedToQueueBefore)
        {
            return entities.Where(p => p.CreatedAt < addedToQueueBefore);
        }
    }
}