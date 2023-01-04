using Darcy_Models;
using Darcy_Models.Forum;
using DarcyWeb_Client.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DarcyWeb_Client.Service
{
    public class ForumService : IForumService
	{
		private readonly HttpClient _client;


		public ForumService(HttpClient client)
		{
			_client = client;
		}

		public async Task<PostDTO> GetPost(int id)
		{
			var response = await _client.GetAsync($"api/forum/getPost/{id}");
			var content = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var post = JsonConvert.DeserializeObject<PostDTO>(content);

				return post;
			}

			else
			{
				var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
				throw new Exception(errorModel.ErrorMessage);
			}
		}

		public async Task<IEnumerable<PostDTO>> GetAllPosts()
		{
			var response = await _client.GetAsync("api/forum/getAllPosts");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var posts = JsonConvert.DeserializeObject<IEnumerable<PostDTO>>(content);

				return posts;
			}

			return Enumerable.Empty<PostDTO>();
		}

		public async Task<PostResponseDTO> Post(PostRequestDTO postingRequestDTO)
		{
			var content = JsonConvert.SerializeObject(postingRequestDTO);
			var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("api/forum/post", bodyContent);
			var contentTemp = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<PostResponseDTO>(contentTemp);

			if (response.IsSuccessStatusCode && result.IsSuccessful is true)
			{
				return new PostResponseDTO()
				{
					IsSuccessful = true,
					PostDTO = result.PostDTO
				};
			}
			else
			{
				return new PostResponseDTO()
				{
					IsSuccessful = false
				};
			}
		}

		public async Task<CommentResponseDTO> Comment(CommentRequestDTO commentRequestDTO)
		{
			var content = JsonConvert.SerializeObject(commentRequestDTO);
			var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("api/forum/comment", bodyContent);
			var contentTemp = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CommentResponseDTO>(contentTemp);

			if (response.IsSuccessStatusCode && result.IsSuccessful is true)
			{
				return new CommentResponseDTO()
				{
					IsSuccessful = true,
					CommentDTO = result.CommentDTO
				};
			}
			else
			{
				return new CommentResponseDTO()
				{
					IsSuccessful = false
				};
			}
		}

		public async Task<IEnumerable<CommentDTO>> GetAllComments(int postId)
		{
			var response = await _client.GetAsync($"api/forum/getAllComments/{postId}");
			
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var comments = JsonConvert.DeserializeObject<IEnumerable<CommentDTO>>(content);

				return comments;
			}

			return Enumerable.Empty<CommentDTO>();
		}
	}
}
