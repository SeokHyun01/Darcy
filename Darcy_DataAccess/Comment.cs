using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_DataAccess
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public int PostId { get; set; }
		[ForeignKey(nameof(PostId))]
		public Post Post { get; set; }
		public DateTime Created { get; set; }
		public string Text { get; set; }
		public int Like { get; set; }
	}
}
