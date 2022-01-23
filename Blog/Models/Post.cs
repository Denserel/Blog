namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
