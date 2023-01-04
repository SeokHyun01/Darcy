using Darcy_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Forum
{
    public class CommentDTO
    {
        public int Id { get; set; }
		public DateTime Created { get; set; }
		public string UserName { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
        public int Like { get; set; }
    }
}
