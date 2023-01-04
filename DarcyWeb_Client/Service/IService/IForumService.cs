using Darcy_Models.Forum;

namespace DarcyWeb_Client.Service.IService
{
    public interface IForumService
	{
		public Task<PostResponseDTO> Post(PostRequestDTO postingRequestDTO);
		public Task<IEnumerable<PostDTO>> GetAllPosts();
		public Task<PostDTO> GetPost(int id);
		public Task<CommentResponseDTO> Comment(CommentRequestDTO commentRequestDTO);
		public Task<IEnumerable<CommentDTO>> GetAllComments(int postId);
	}
}
