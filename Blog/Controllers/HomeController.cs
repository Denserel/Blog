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
            return View();
        }

        public IActionResult Post ()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit ()
        {
            return View(new Post());
        }

        [HttpPost]
        public async  Task<IActionResult> Edit(Post post)
        {
            repository.addPost(post);

            if (await repository.SaveChanges())
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }
    }
}
