using Darcy_Models.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository.IRepository
{
    public interface ICameraRepository
    {
        public Task<CameraDTO> Create(CameraDTO objDTO);
        public Task<CameraDTO> Update(CameraDTO objDTO);
        public Task<int> Delete(int id);
        public Task<CameraDTO?> Get(int id);
        public Task<IEnumerable<CameraDTO>> GetAll(string? userId = null);
    }
}
