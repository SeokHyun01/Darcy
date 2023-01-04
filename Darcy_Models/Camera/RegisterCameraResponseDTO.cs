using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Camera
{
    public class RegisterCameraResponseDTO
    {
        public bool IsRegisterationSuccessful { get; set; }
        public string Error { get; set; }
        public CameraDTO CameraDTO { get; set; }
    }
}
