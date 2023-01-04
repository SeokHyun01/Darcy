using Darcy_Models.Camera;

namespace DarcyWeb_Client.Service.IService
{
    public interface ICameraService
	{
		public Task<RegisterCameraResponseDTO> Register(RegisterCameraRequestDTO registerCameraRequestDTO);
	}
}
