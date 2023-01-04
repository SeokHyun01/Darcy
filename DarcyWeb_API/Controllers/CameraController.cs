using Darcy_Business.Repository.IRepository;
using Darcy_Common;
using Darcy_DataAccess;
using Darcy_Models;
using Darcy_Models.Camera;
using DarcyWeb_API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DarcyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
	[ApiController]
	public class CameraController : Controller
	{
		private readonly ILogger<CameraController> _logger;
		private readonly ICameraRepository _cameraRepository;
		private readonly UserManager<ApplicationUser> _userManager;


		public CameraController(
			ILogger<CameraController> logger,
			ICameraRepository cameraRepository,
			UserManager<ApplicationUser> userManager
			)
		{
			_logger = logger;
			_cameraRepository = cameraRepository;
			_userManager = userManager;
		}


		[HttpPost]
		[ActionName("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterCameraRequestDTO registerCameraRequest)
		{
			if (registerCameraRequest is null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = await _userManager.FindByIdAsync(registerCameraRequest.UserId);
			
			if (user is not null)
			{
				var camera = new CameraDTO()
				{
					UserId = registerCameraRequest.UserId,
					Name = registerCameraRequest.Name,
					Thumbnail = registerCameraRequest.Thumbnail
				};

				await _cameraRepository.Create(camera);

				return Ok(new RegisterCameraResponseDTO()
				{
					IsRegisterationSuccessful = true,
					CameraDTO = camera
				});

			}
			else
			{
				return BadRequest(new RegisterCameraResponseDTO()
				{
					IsRegisterationSuccessful = false,
					Error = "유저가 존재하지 않습니다."
				});
			}
		}
	}
}
