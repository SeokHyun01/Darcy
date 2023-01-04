using Darcy_Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository.IRepository
{
    public interface IEventRepository
	{
		public Task<EventDTO> Create(EventDTO objDTO);
		public Task<int> Delete(int id);
		public Task<EventDTO?> Get(int id);
		public Task<IEnumerable<EventDTO>> GetAll();
		public Task<IEnumerable<EventDTO>> GetAllByCameraId(int cameraId);
		public Task<IEnumerable<EventDTO>> GetAllByUserId(string userId);
	}
}
