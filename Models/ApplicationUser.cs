using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRegProfile.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Player1_First_Name { get; set; }
        public string Player1_Last_Name { get; set; }
        public string Player1_Team { get; set; }
        public string Player2_First_Name { get; set; }
        public string Player2_Last_Name { get; set; }
        public string Player2_Team { get; set; }



    }
}
