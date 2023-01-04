using Darcy_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Models.Forum
{
    public class PostRequestDTO
    {
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "제목을 입력해주세요")]
        public string Title { get; set; }
        [Required(ErrorMessage = "본문을 입력해주세요")]
        public string Text { get; set; }
        [Required(ErrorMessage = "모집 인원 수를 입력해주세요")]
        public int Personnel { get; set; }
        public string Status { get; set; }
    }
}
