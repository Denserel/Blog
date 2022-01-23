﻿using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category) ? repository.getAllPosts() : repository.getAllPosts(category);

            return View(posts);
        }

        public IActionResult Post (int id)
        {
            var post = repository.getPost(id);

            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image (string image)
        {
            var mime = image.Substring(image.LastIndexOf('.')+ 1);

            return new FileStreamResult(fileManager.GetImage(image), $"image/{mime}");
        }
    }
}
