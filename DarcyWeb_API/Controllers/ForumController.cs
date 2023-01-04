using Darcy_Business.Repository;
using Darcy_Business.Repository.IRepository;
using Darcy_DataAccess;
using Darcy_Models;
using Darcy_Models.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DarcyWeb_API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ForumController : Controller
	{
		private readonly ILogger<ForumController> _logger;
		private readonly IPostRepository _postRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public ForumController(ILogger<ForumController> logger, IPostRepository postRepository, ICommentRepository commentRepository, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_postRepository = postRepository;
			_commentRepository = commentRepository;
			_userManager = userManager;
		}

		[HttpPost]
		[ActionName("Post")]
		public async Task<IActionResult> Post([FromBody] PostRequestDTO postingRequest)
		{
			if (postingRequest is null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = await _userManager.FindByEmailAsync(postingRequest.UserName);

			if (user is not null)
			{
				var post = new PostDTO()
				{
					UserName = postingRequest.UserName,
					Created = postingRequest.Created,
					Title = postingRequest.Title,
					Text = postingRequest.Text,
					Personnel = postingRequest.Personnel,
					Status = postingRequest.Status
				};

				await _postRepository.Create(post);

				return Ok(new PostResponseDTO()
				{
					IsSuccessful = true,
					PostDTO = post
				});

			}
			else
			{
				return BadRequest(new PostResponseDTO()
				{
					IsSuccessful = false,
					Error = "유저가 존재하지 않습니다."
				});
			}
		}

		[HttpGet]
		[ActionName("GetAllPosts")]
		public async Task<IActionResult> GetAllPosts()
		{
			return Ok(await _postRepository.GetAll());
		}

		[HttpGet("{postId}")]
		[ActionName("GetAllComments")]
		public async Task<IActionResult> GetAllComments(int? postId)
		{
			if (postId == null || postId <= 0)
			{
				return BadRequest(new ErrorModelDTO()
				{
					ErrorMessage = "Invalid Id",
					StatusCode = StatusCodes.Status400BadRequest
				});
			}

			var comments = await _commentRepository.GetAllByPostId(postId.Value);

			return Ok(comments);
		}

		[HttpGet("{postId}")]
        [ActionName("GetPost")]
        public async Task<IActionResult> GetPost(int? postId)
        {
            if (postId == null || postId <= 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var post = await _postRepository.Get(postId.Value);
            if (post == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(post);
        }

		[HttpPost]
		[ActionName("Comment")]
		public async Task<IActionResult> Comment([FromBody] CommentRequestDTO commentRequest)
		{
			if (commentRequest is null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = await _userManager.FindByEmailAsync(commentRequest.UserName);

			if (user is not null)
			{
				var comment = new CommentDTO()
				{
					Created = commentRequest.Created,
					UserName = commentRequest.UserName,
					PostId = commentRequest.PostId,
					Text = commentRequest.Text
				};

				await _commentRepository.Create(comment);

				return Ok(new CommentResponseDTO()
				{
					IsSuccessful = true,
					CommentDTO = comment
				});

			}
			else
			{
				return BadRequest(new PostResponseDTO()
				{
					IsSuccessful = false,
					Error = "유저가 존재하지 않습니다."
				});
			}
		}
	}
}
