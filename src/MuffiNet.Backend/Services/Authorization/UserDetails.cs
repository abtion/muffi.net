using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuffiNet.Backend.Services.Authorization
{
    public class UserDetails
    {
        public string Email { get; init; }

        public UserDetails(string email)
        {
            Email = email;
        }
    }
}
