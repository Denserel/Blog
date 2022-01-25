using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;
        private readonly IFileManager fileManager;

        public HomeController(IRepository repository, IFileManager fileManager)
        {
            this.repository = repository;
            this.fileManager = fileManager;
        }

        public IActionResult Index(string searchString)
        {
            var posts = string.IsNullOrEmpty(searchString) ? repository.getAllPosts() : repository.getAllPosts(searchString);

            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = repository.getPost(id);

            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);

            return new FileStreamResult(fileManager.GetImage(image), $"image/{mime}");
        }

        public async Task<IActionResult> Comment(CommentViewModel commentViewModel)
        {
            var post = repository.getPost(commentViewModel.PostId);
            if (ModelState.IsValid)
            {
                if (commentViewModel.MainCommentId.HasValue)
                {

                }
                else
                {
                    var comment = new MainComment
                    {
                        Message = commentViewModel.Message,
                        DateCreated = DateTime.Now
                    };

                    post.Comments.Add(comment);

                }

                repository.updatePost(post);
                await repository.SaveChangesAsync();
            }

            return RedirectToAction("Post", new { id = commentViewModel.PostId } );
        }
    }
}
