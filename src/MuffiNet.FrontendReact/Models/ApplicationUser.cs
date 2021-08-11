using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MuffiNet.FrontendReact.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(200)]
        public string FullName { get; set; }
    }
}