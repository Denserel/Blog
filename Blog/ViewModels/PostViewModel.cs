using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Tags { get; set; }
        [Required]
        public string Category { get; set; }
        public string CurrentImage { get; set; }
        public IFormFile Image { get; set; }
    }
}
