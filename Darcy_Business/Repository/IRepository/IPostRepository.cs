using Darcy_Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository.IRepository
{
    public interface IPostRepository
	{
		public Task<PostDTO> Create(PostDTO objDTO);
		public Task<PostDTO> Update(PostDTO objDTO);
		public Task<int> Delete(int id);
		public Task<PostDTO?> Get(int id);
		public Task<IEnumerable<PostDTO>> GetAll(string? userId = null);
	}
}
