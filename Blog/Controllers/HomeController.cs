using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var posts = repository.getPost(null);

            return View(posts);
        }

        public IActionResult Post (int id)
        {
            var post = repository.getPost(id).FirstOrDefault();

            return View(post);
        }

        [HttpGet]
        public IActionResult Edit (int? id)
        {
            if (id == null)
            {
                return View(new Post());
            }

            var post = repository.getPost(id).FirstOrDefault();
            return View(post);
        }

        [HttpPost]
        public async  Task<IActionResult> Edit(Post post)
        {
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
        public async Task <IActionResult> Delete(int id)
        {
            repository.deletePost(id);
            await repository.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}
