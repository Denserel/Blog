using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IRepository repository;
        private readonly IFileManager fileManager;
        private readonly IMapper mapper;

        public AdminController(
            IRepository repository, 
            IFileManager fileManager, 
            IMapper mapper)
        {
            this.repository = repository;
            this.fileManager = fileManager;
            this.mapper = mapper;
        }

        public async Task <IActionResult> Index(string searchString, int pageSize = 20, int pageIndex = 1)
        {
            ViewData["searchString"] = searchString;
            var posts = await PaginatedList<Post>.CreateAsync(await repository.getAllPostsAsync(searchString), pageSize, pageIndex);

            return View(posts);
        }

        [HttpGet]
        public async Task <IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }

            var post = await repository.getPostAsync((int) id);
            var postVm = mapper.Map<Post, PostViewModel>(post);
            postVm.CurrentImage = post.Image;
            return View(postVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postVm)
        {
            var post = mapper.Map<PostViewModel,Post>(postVm);

            if(postVm.Image == null)
            {
                post.Image = postVm.CurrentImage;
            }
            else
            {
                post.Image = await fileManager.SaveImageAsync(postVm.Image);
                
                if(!string.IsNullOrEmpty(postVm.CurrentImage))
                {
                    fileManager.RemoveImage(postVm.CurrentImage);
                }
            }

            if (postVm.Id > 0)
            {
                repository.updatePost(post);
            }
            else
            {
               await repository.addPost(post);
            }

            if (await repository.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }

            return View(postVm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await repository.getPostAsync(id);

            fileManager.RemoveImage(post.Image);

            
            await repository.deletePostAsync(id);
            await repository.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
