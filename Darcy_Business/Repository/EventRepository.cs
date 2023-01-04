using AutoMapper;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_DataAccess.Data;
using Darcy_DataAccess.ViewModel;
using Darcy_Models.Event;
using Darcy_Models.Forum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository
{
    public class EventRepository : IEventRepository
	{
		private readonly DarcyAppDbContext _db;
		private readonly IMapper _mapper;


		public EventRepository(DarcyAppDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}


		public async Task<EventDTO> Create(EventDTO objDTO)
		{
			try
			{
				var obj = _mapper.Map<EventDTO, Event>(objDTO);
				_db.EventHeaders.Add(obj.EventHeader);
				await _db.SaveChangesAsync();

				foreach (var detail in obj.EventDetails)
				{
					detail.EventHeaderId = obj.EventHeader.Id;
				}
				_db.EventDetails.AddRange(obj.EventDetails);
				await _db.SaveChangesAsync();

				return new EventDTO()
				{
					EventHeader = _mapper.Map<EventHeader, EventHeaderDTO>(obj.EventHeader),
					EventDetails = _mapper.Map<IEnumerable<EventDetail>, IEnumerable<EventDetailDTO>>(obj.EventDetails)
				};
			}

			catch (Exception ex)
			{
				throw;
			}

			return objDTO;
		}

		public async Task<int> Delete(int id)
		{
			var objHeader = await _db.EventHeaders.FirstOrDefaultAsync(u => u.Id == id);
			if (objHeader != null)
			{
				IEnumerable<EventDetail> objDetails = _db.EventDetails.Where(u => u.EventHeaderId == id);

				_db.EventDetails.RemoveRange(objDetails);
				_db.EventHeaders.Remove(objHeader);
				return _db.SaveChanges();
			}
			return 0;
		}

		public async Task<EventDTO> Get(int id)
		{
			Event evnt = new()
			{
				EventHeader = _db.EventHeaders.FirstOrDefault(u => u.Id == id),
				EventDetails = _db.EventDetails.Where(u => u.EventHeaderId == id),
			};

			if (evnt != null)
			{
				return _mapper.Map<Event, EventDTO>(evnt);
			}
			return new EventDTO();
		}

		public async Task<IEnumerable<EventDTO>> GetAll()
		{
			List<Event> EventsFromDb = new List<Event>();

			IEnumerable<EventHeader> evntHeaderList = _db.EventHeaders;
			IEnumerable<EventDetail> evntDetailList = _db.EventDetails;

			foreach (var header in evntHeaderList)
			{
				Event evnt = new()
				{
					EventHeader = header,
					EventDetails = evntDetailList.Where(u => u.EventHeaderId == header.Id),
				};
				EventsFromDb.Add(evnt);
			}

			return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(EventsFromDb);
		}

		public async Task<IEnumerable<EventDTO>> GetAllByCameraId(int cameraId)
		{
			List<Event> EventsFromDb = new List<Event>();

			if (cameraId > 0)
			{
				IEnumerable<EventHeader> evntHeaderList = _db.EventHeaders.Where(u => u.CameraId == cameraId);

				foreach (var header in evntHeaderList)
				{
					Event evnt = new()
					{
						EventHeader = header,
						EventDetails = _db.EventDetails.Where(u => u.EventHeaderId == header.Id),
					};
					EventsFromDb.Add(evnt);
				}
			}

			return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(EventsFromDb);
		}

		public async Task<IEnumerable<EventDTO>> GetAllByUserId(string userId)
		{
			List<Event> EventsFromDb = new List<Event>();

			if (!string.IsNullOrEmpty(userId))
			{
				IEnumerable<EventHeader> evntHeaderList = _db.EventHeaders.Where(u => u.UserId == userId);

				foreach (var header in evntHeaderList)
				{
					Event evnt = new()
					{
						EventHeader = header,
						EventDetails = _db.EventDetails.Where(u => u.EventHeaderId == header.Id),
					};
					EventsFromDb.Add(evnt);
				}
			}

			return _mapper.Map<IEnumerable<Event>, IEnumerable<EventDTO>>(EventsFromDb);
		}
	}
}