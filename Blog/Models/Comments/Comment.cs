using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string User { get; set; }
    }
}
