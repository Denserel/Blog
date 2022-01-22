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
            var posts = repository.getPost(null);

            return View(posts);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }

            var post = repository.getPost(id).FirstOrDefault();
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
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
                Body = postVm.Body
            };

            if(postVm.Image == null)
            {
                post.Image = postVm.CurrentImage;
            }
            else
            {
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

            if (await repository.SaveChanges())
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            repository.deletePost(id);
            await repository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
