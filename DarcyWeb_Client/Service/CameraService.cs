using Darcy_Common;
using Darcy_Models.Camera;
using DarcyWeb_Client.Service.IService;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DarcyWeb_Client.Service
{
    public class CameraService : ICameraService
	{
		private readonly HttpClient _client;


		public CameraService(HttpClient client)
		{
			_client = client;
		}


		public async Task<RegisterCameraResponseDTO> Register(RegisterCameraRequestDTO registerCameraRequestDTO)
		{
			var content = JsonConvert.SerializeObject(registerCameraRequestDTO);
			var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("api/camera/register", bodyContent);
			var contentTemp = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<RegisterCameraResponseDTO>(contentTemp);

			if (response.IsSuccessStatusCode && result.IsRegisterationSuccessful is true)
			{
				return new RegisterCameraResponseDTO()
				{
					IsRegisterationSuccessful = true,
					CameraDTO = result.CameraDTO
				};
			}
			else
			{
				return new RegisterCameraResponseDTO()
				{
					IsRegisterationSuccessful = false
				};
			}
		}
	}
}
