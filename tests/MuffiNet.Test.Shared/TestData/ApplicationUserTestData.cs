using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;

namespace MuffiNet.Test.Shared.TestData
{
    public static class ApplicationUserTestData
    {
        public static ApplicationUser CreateApplicationUser()
        {
            return new ApplicationUser()
            {
                FullName = "Donald Duck",
                UserName = "donald@duck.disney",
                Id = "e0169f6f-c521-4d75-9144-a46c692af355"
            };
        }
    }
}