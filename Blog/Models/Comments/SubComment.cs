using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models.Comments
{
    public class SubComment : Comment
    {
        [ForeignKey("MainCommentId")]
        public int MainCommentId { get; set; }
    }
}
