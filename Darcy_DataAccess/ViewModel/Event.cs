using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_DataAccess.ViewModel
{
    public class Event
    {
        public EventHeader EventHeader { get; set; }
        public IEnumerable<EventDetail> EventDetails { get; set; }
    }
}
