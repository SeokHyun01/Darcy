using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Camera
{
	public class UpdateCameraDTO
	{
		public int CameraId { get; set; }
		public string Thumbnail { get; set; }
		public int Degree { get; set; }
	}
}
