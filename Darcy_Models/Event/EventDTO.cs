using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Event
{
    public class EventDTO
    {
        public EventHeaderDTO EventHeader { get; set; }
        public IEnumerable<EventDetailDTO> EventDetails { get; set; }
    }
}
