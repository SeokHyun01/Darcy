using AutoMapper;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_DataAccess.Data;
using Darcy_Models;
using Darcy_Models.Forum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository
{
    public class CommentRepository : ICommentRepository
	{
		private readonly DarcyAppDbContext _db;
		private readonly IMapper _mapper;


		public CommentRepository(DarcyAppDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<CommentDTO> Create(CommentDTO objDTO)
		{
			var obj = _mapper.Map<CommentDTO, Comment>(objDTO);

			var addedObj = _db.Comments.Add(obj);
			await _db.SaveChangesAsync();

			return _mapper.Map<Comment, CommentDTO>(addedObj.Entity);
		}

		public async Task<int> Delete(int id)
		{
			var obj = await _db.Comments.FirstOrDefaultAsync(u => u.Id == id);
			if (obj is not null)
			{
				_db.Comments.Remove(obj);
				return await _db.SaveChangesAsync();
			}
			return 0;
		}

		public async Task<CommentDTO?> Get(int id)
		{
			var obj = await _db.Comments.FirstOrDefaultAsync(u => u.Id == id);
			if (obj is not null)
			{
				return _mapper.Map<Comment, CommentDTO>(obj);
			}

			return null;
		}

		public async Task<IEnumerable<CommentDTO>> GetAllByPostId(int postId)
		{
			if (postId > 0)
			{
				return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(_db.Comments.Where(u => u.PostId == postId));
			}
			else
			{
				return Enumerable.Empty<CommentDTO>();
			}
		}

		public async Task<IEnumerable<CommentDTO>> GetAll()
		{
			return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(_db.Comments);
		}

		public async Task<CommentDTO> Update(CommentDTO objDTO)
		{
			var objFromDb = await _db.Comments.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
			if (objFromDb is not null)
			{
				// Update
				_db.Comments.Update(objFromDb);
				await _db.SaveChangesAsync();
				return _mapper.Map<Comment, CommentDTO>(objFromDb);
			}
			return objDTO;
		}
	}
}
