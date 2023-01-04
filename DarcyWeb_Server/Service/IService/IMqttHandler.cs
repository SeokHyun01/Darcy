namespace DarcyWeb_Server.Service.IService
{
	public interface IMqttHandler : IAsyncDisposable, IDisposable
	{
		public Task Listen();
	}
}
