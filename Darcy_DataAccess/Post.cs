using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_DataAccess
{
	public class Post
	{
		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public DateTime Created { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public int Personnel { get; set; }
		public string Status { get; set; }
		public ICollection<Comment> Comments { get; set; }
	}
}
