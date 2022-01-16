using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
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
        public IActionResult Edit(Post post)
        {


            return RedirectToAction("Index");
        }
    }
}
