using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Camera
{
	public class RegisterCameraRequestDTO
	{
		public string UserId { get; set; }
		public string Name { get; set; }
		public string Thumbnail { get; set; }
	}
}
