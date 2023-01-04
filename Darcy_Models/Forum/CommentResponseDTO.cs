using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Forum
{
    public class CommentResponseDTO
    {
		public bool IsSuccessful { get; set; }
		public string Error { get; set; }
		public CommentDTO CommentDTO { get; set; }
	}
}
