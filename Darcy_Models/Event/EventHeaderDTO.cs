using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Event
{
    public class EventHeaderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CameraId { get; set; }
        public string Created { get; set; }
        public string Image { get; set; }
    }
}
