using Darcy_Business.Repository;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_DataAccess.ViewModel;
using Darcy_Models.Camera;
using Darcy_Models.Event;
using DarcyWeb_Server.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Server;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace DarcyWeb_Server.Service
{
	public class MqttHandler : IMqttHandler
	{
		private readonly IEventRepository _eventRepository;
		private readonly ICameraRepository _cameraRepository;
		private readonly ILogger<MqttHandler> _logger;

		private IMqttClient? MqttClient { get; set; }


		public MqttHandler(ILogger<MqttHandler> logger, IEventRepository eventRepository, ICameraRepository cameraRepository)
		{
			_logger = logger;
			_eventRepository = eventRepository;
			_cameraRepository = cameraRepository;
		}


		public async Task Listen()
		{
			var mqttFactory = new MqttFactory();

			MqttClient = mqttFactory.CreateMqttClient();

			var mqttClientOptions = new MqttClientOptionsBuilder()
				.WithTcpServer("ictrobot.hknu.ac.kr", 8085)
				//.WithTcpServer("192.168.56.1", 1883)
				.WithClientId("Darcy")
				.Build();

			MqttClient.ApplicationMessageReceivedAsync += async e =>
			{
				var payload = e.ApplicationMessage.Payload;

				if (payload is not null)
				{
					_logger.LogInformation($"Payload: {payload}");

					if (e.ApplicationMessage.Topic == "event/create")
					{
						CreateEventDTO? createEvent = JsonSerializer.Deserialize<CreateEventDTO>(payload);
						if (createEvent != null)
						{
							var evnt = new EventDTO
							{
								EventHeader = new EventHeaderDTO
								{
									UserId = createEvent.UserId,
									CameraId = createEvent.CameraId,
									Created = createEvent.Created,
									Image = createEvent.Image
								},
								EventDetails = createEvent.EventDetails
							};

							await _eventRepository.Create(evnt);
							_logger.LogInformation($"카메라 {evnt.EventHeader.CameraId}에서 이벤트가 발생했습니다.");
						}
					}

					else if (e.ApplicationMessage.Topic == "camera/update/degree/syn")
					{
						var updateCamera = JsonSerializer.Deserialize<UpdateCameraDTO>(payload);
						if (updateCamera != null)
						{
							var camera = new CameraDTO
							{
								Id = updateCamera.CameraId,
								Degree = updateCamera.Degree
							};

							await _cameraRepository.Update(camera);
							_logger.LogInformation($"카메라 {updateCamera.CameraId}이 업데이트 됐습니다.");
						}
					}
				}
				await Task.CompletedTask;
			};

			await MqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

			var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
				.WithTopicFilter(f =>
				{
					f.WithTopic("event/create");
				})
				.WithTopicFilter(f =>
				{
					f.WithTopic("camera/update/degree/syn");
				})
				.Build();

			await MqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
		}


		private bool _disposedValue;

		public void Dispose() => Dispose(true);


		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					MqttClient?.Dispose();
					_logger.LogInformation("MqttHandler 인스턴스가 dispose 됐습니다.");
				}

				_disposedValue = true;
			}
		}


		public async ValueTask DisposeAsync()
		{
			if (MqttClient != null && MqttClient.IsConnected)
			{
				await MqttClient.DisconnectAsync();
				_logger.LogInformation("클라이언트 Darcy의 연결이 끊어졌습니다.");
			}
		}
	}
}
