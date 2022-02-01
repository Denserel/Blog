using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public int PostId { get; set; }
        public int? MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
        public string User { get; set; }
    }
}
