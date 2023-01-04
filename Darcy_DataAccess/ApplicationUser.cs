using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_DataAccess
{
    public class ApplicationUser : IdentityUser
    {
		public int Age { get; set; }
		public string Name { get; set; }
		public string Gender { get; set; }
        public string Major { get; set; }
    }
}
