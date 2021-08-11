using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace MuffiNet.FrontendReact.Models
{
    [Index(nameof(CallEndedAt), nameof(CreatedAt), Name = "IX_SupportTicket_CallEndedAt_CreatedAt")]
    [Index(nameof(CreatedAt), nameof(CallStartedAt), nameof(CallEndedAt), Name = "IX_SupportTicket_CreatedAt_CallStartedAt_CallEndedAt")]
    public class SupportTicket
    {
        public int Id { get; set; }

        [Required]
        public Guid SupportTicketId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CallStartedAt { get; set; }

        public DateTime? CallEndedAt { get; set; }

        [MaxLength(200)]
        public string TechnicianUserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(30)]
        public string CustomerPhone { get; set; }

        [Required]
        [MaxLength(300)]
        public string CustomerEmail { get; set; }

        [MaxLength(30)]
        public string Brand { get; set; }

        [MaxLength(200)]
        public string TwilioRoomName { get; set; }

        [MaxLength(500)]
        public string TwilioVideoGrantForTechnicianToken { get; set; }

        [MaxLength(500)]
        public string TwilioVideoGrantForCustomerToken { get; set; }

        [MaxLength(200)]
        public string TwilioRoomSid { get; set; }

        [MaxLength(10)]
        public string OssId { get; set; }
    }
}
