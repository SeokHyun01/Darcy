using Darcy_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Event
{
    public class CreateEventDTO
    {
        public string UserId { get; set; }
        public int CameraId { get; set; }
        public string Created { get; set; }
        public string Image { get; set; }
        public IEnumerable<EventDetailDTO> EventDetails { get; set; }
    }
}
