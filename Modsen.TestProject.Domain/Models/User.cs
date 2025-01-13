using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modsen.TestProject.Domain.Models
{
    public class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
    }

}
