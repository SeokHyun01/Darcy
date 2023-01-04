using Darcy_DataAccess;
using Darcy_Models.Camera;
using DarcyWeb_Server.Service.IService;
using System.Linq;
using System.Text.Json;

namespace DarcyWeb_Server.Service
{
	public class HostedMqttClient : BackgroundService, IDisposable
	{
		public IServiceProvider Services { get; }
		public IMqttHandler? mqttHandler;

		private readonly ILogger<HostedMqttClient> _logger;

		public HostedMqttClient(ILogger<HostedMqttClient> logger, IServiceProvider services)
		{
			_logger = logger;
			Services = services;
		}


		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Mqtt handler service is running.");

			using (var scope = Services.CreateScope())
			{
				mqttHandler = scope.ServiceProvider.GetRequiredService<IMqttHandler>();
				
				await mqttHandler.Listen();

				SpinWait.SpinUntil(() => stoppingToken.IsCancellationRequested, -1);

				await mqttHandler.DisposeAsync();
			}
		}
	}
}
