using Darcy_Models.Authentication;

namespace DarcyWeb_Client.Service.IService
{
    public interface IAuthenticationService
	{
		public Task<SignUpResponseDTO> SignUpUser(SignUpRequestDTO signUpRequestDTO);
		public Task<SignInResponseDTO> Login(SignInRequestDTO signInRequestDTO);
		public Task Logout();
	}
}
