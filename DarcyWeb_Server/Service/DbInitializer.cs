using Darcy_Common;
using Darcy_DataAccess.Data;
using DarcyWeb_Server.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarcyWeb_Server.Service
{
	public class DbInitializer : IDbInitializer
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly DarcyAppDbContext _db;
		private readonly ILogger<DbInitializer> _logger;

		public DbInitializer(UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			DarcyAppDbContext db,
			ILogger<DbInitializer> logger)
		{
			_db = db;
			_roleManager = roleManager;
			_userManager = userManager;
			_logger = logger;
		}

		public async Task Initialize()
		{
			try
			{
				if (_db.Database.GetPendingMigrations().Count() > 0)
				{
					_db.Database.Migrate();
				}
				if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
				{
					await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
					await _roleManager.CreateAsync(new IdentityRole(SD.Role_Client));
				}
				else
				{
					return;
				}

				IdentityUser user = new()
				{
					UserName = "admin@darcy.com",
					Email = "admin@darcy.com",
					EmailConfirmed = true
				};

				await _userManager.CreateAsync(user, "95fur6u?_!deQ%8");

				await _userManager.AddToRoleAsync(user, SD.Role_Admin);

			}
			catch (Exception ex)
			{
				_logger.LogInformation(ex.GetType().FullName);
				_logger.LogInformation(ex.Message);
			}
		}
	}
}
