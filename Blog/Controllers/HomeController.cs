using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;
        private readonly IFileManager fileManager;
        private readonly IMapper mapper;

        public HomeController(IRepository repository, 
            IFileManager fileManager,
            IMapper mapper)
        {
            this.repository = repository;
            this.fileManager = fileManager;
            this.mapper = mapper;
        }

        public async Task <IActionResult> Index(string searchString, int pageSize = 5, int pageIndex = 1)
        {
            ViewData["searchString"] = searchString;
            var posts = await PaginatedList<Post>.CreateAsync(await repository.getAllPostsAsync(searchString), pageSize, pageIndex);

            return View(posts);
        }

        public async Task <IActionResult> Post(int id)
        {
            var post = await repository.getPostAsync(id);

            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var contentType = string.Empty;
            new FileExtensionContentTypeProvider().TryGetContentType(image, out contentType);

            return new FileStreamResult(fileManager.GetImage(image), contentType);
        }

        public async Task<IActionResult> Comment(CommentViewModel commentViewModel)
        {
            var post = await repository.getPostAsync(commentViewModel.PostId);
            if (ModelState.IsValid)
            {
                if (commentViewModel.MainCommentId.HasValue)
                {

                }
                else
                {
                    var comment = mapper.Map<CommentViewModel, MainComment>(commentViewModel);
                    
                    post.Comments.Add(comment);
                }

                repository.updatePost(post);
                await repository.SaveChangesAsync();
            }

            return RedirectToAction("Post", new { id = commentViewModel.PostId } );
        }
    }
}
