using AutoMapper;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_DataAccess.Data;
using Darcy_Models.Forum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository
{
    public class PostRepository : IPostRepository
	{
		private readonly DarcyAppDbContext _db;
		private readonly IMapper _mapper;


		public PostRepository(DarcyAppDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<PostDTO> Create(PostDTO objDTO)
		{
			var obj = _mapper.Map<PostDTO, Post>(objDTO);
			var addedObj = _db.Posts.Add(obj);
			await _db.SaveChangesAsync();

			return _mapper.Map<Post, PostDTO>(addedObj.Entity);
		}

		public async Task<int> Delete(int id)
		{
			var obj = await _db.Posts.FirstOrDefaultAsync(u => u.Id == id);
			if (obj is not null)
			{
				_db.Posts.Remove(obj);
				return await _db.SaveChangesAsync();
			}
			return 0;
		}

		public async Task<PostDTO?> Get(int id)
		{
			var obj = await _db.Posts.Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == id);
			if (obj is not null)
			{
				return _mapper.Map<Post, PostDTO>(obj);
			}
			return null;

		}

		public async Task<IEnumerable<PostDTO>> GetAll(string? userName = null)
		{
			if (!string.IsNullOrEmpty(userName))
			{
				return _mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>
					(_db.Posts.Where(u => u.UserName == userName).Include(u => u.Comments));
			}
			else
			{
				return _mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(_db.Posts.Include(u => u.Comments));
			}
		}

		public async Task<PostDTO> Update(PostDTO objDTO)
		{
			var objFromDb = await _db.Posts.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
			if (objFromDb is not null)
			{
				// Update
				_db.Posts.Update(objFromDb);
				await _db.SaveChangesAsync();
				return _mapper.Map<Post, PostDTO>(objFromDb);
			}
			return objDTO;
		}
	}
}
