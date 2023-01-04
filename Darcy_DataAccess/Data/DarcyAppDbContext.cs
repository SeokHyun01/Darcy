using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_DataAccess.Data
{
    public class DarcyAppDbContext : IdentityDbContext
    {
        public DarcyAppDbContext(DbContextOptions<DarcyAppDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Camera> Cameras { get; set; }

        public DbSet<EventHeader> EventHeaders { get; set; }
		public DbSet<EventDetail> EventDetails { get; set; }

		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}
