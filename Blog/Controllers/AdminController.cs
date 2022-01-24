using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IRepository repository;
        private readonly IFileManager fileManager;

        public AdminController(IRepository repository, IFileManager fileManager)
        {
            this.repository = repository;
            this.fileManager = fileManager;
        }

        public IActionResult Index()
        {
            var posts = repository.getAllPosts();

            return View(posts);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }

            var post = repository.getPost((int) id);
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Description = post.Description,
                Tags = post.Tags,
                Category = post.Category,
                CurrentImage = post.Image
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postVm)
        {
            Post post = new ()
            {
                Id = postVm.Id,
                Title = postVm.Title,
                Body = postVm.Body,
                Description = postVm.Description,
                Tags = postVm.Tags,
                Category = postVm.Category
            };

            if(postVm.Image == null)
            {
                post.Image = postVm.CurrentImage;
            }
            else
            {
                if (!string.IsNullOrEmpty(postVm.CurrentImage))
                {
                    fileManager.RemoveImage(postVm.CurrentImage);
                }

                post.Image = await fileManager.SaveImage(postVm.Image);
            }

            if (post.Id > 0)
            {
                repository.updatePost(post);
            }
            else
            {
                repository.addPost(post);
            }

            if (await repository.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            fileManager.RemoveImage(repository.getPost(id).Image);

            repository.deletePost(id);
            await repository.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
