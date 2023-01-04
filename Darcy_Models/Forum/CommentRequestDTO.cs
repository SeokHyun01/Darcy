using Darcy_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Forum
{
    public class CommentRequestDTO
    {
		public DateTime Created { get; set; }
		public string UserName { get; set; }
		public int PostId { get; set; }
		[Required(ErrorMessage = "본문을 입력해주세요")]
		public string Text { get; set; }

	}
}
