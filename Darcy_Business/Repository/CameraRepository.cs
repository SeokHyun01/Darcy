using AutoMapper;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_DataAccess.Data;
using Darcy_Models.Camera;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository
{
    public class CameraRepository : ICameraRepository
    {
        private readonly DarcyAppDbContext _db;
        private readonly IMapper _mapper;

        public CameraRepository(DarcyAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<CameraDTO> Create(CameraDTO objDTO)
        {
            var obj = _mapper.Map<CameraDTO, Camera>(objDTO);

            var addedObj = _db.Cameras.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Camera, CameraDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Cameras.FirstOrDefaultAsync(u => u.Id == id);
            if (obj is not null)
            {
                _db.Cameras.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CameraDTO?> Get(int id)
        {
            var obj = await _db.Cameras.FirstOrDefaultAsync(u => u.Id == id);
            if (obj is not null)
            {
                return _mapper.Map<Camera, CameraDTO>(obj);
            }
            return null;
        }

        public async Task<IEnumerable<CameraDTO>> GetAll(string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _mapper.Map<IEnumerable<Camera>, IEnumerable<CameraDTO>>
                    (_db.Cameras.Where(u => u.UserId == userId));
            }
            else
            {
                return _mapper.Map<IEnumerable<Camera>, IEnumerable<CameraDTO>>(_db.Cameras);
            }
        }

        public async Task<CameraDTO> Update(CameraDTO objDTO)
        {
            var objFromDb = await _db.Cameras.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb is not null)
            {
				// Update
                objFromDb.Degree = objDTO.Degree;

				_db.Cameras.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Camera, CameraDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
