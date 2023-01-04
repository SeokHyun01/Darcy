using Darcy_Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Repository.IRepository
{
    public interface ICommentRepository
    {
        public Task<CommentDTO> Create(CommentDTO objDTO);
        public Task<CommentDTO> Update(CommentDTO objDTO);
        public Task<int> Delete(int id);
        public Task<CommentDTO?> Get(int id);
        public Task<IEnumerable<CommentDTO>> GetAll();
        public Task<IEnumerable<CommentDTO>> GetAllByPostId(int postId);
    }
}
