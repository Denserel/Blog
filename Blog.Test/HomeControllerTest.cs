using AutoMapper;
using Blog.Controllers;
using Blog.Data;
using Blog.Mapper;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Test
{
    //private readonly Mock<IRepository> 

    public class HomeControllerTest
    {
        private IMapper mapper;
        private IFileManager fileManager;
        private readonly Random random = new();
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();

        [Fact]
        public async Task Index_OnGetCall_ReturnAllPosts ()
        {
            var posts = new List<Post>() { Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random) };
            var searchString = string.Empty;
            mockRepository.Setup(repository => repository.getAllPostsAsync(searchString)).ReturnsAsync(posts);
            var conroller = new HomeController(mockRepository.Object, fileManager, mapper);

            var result = await conroller.Index(searchString);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Post>> (viewResult.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Post_ProvidingPostId_ReturnPost()
        {
            var post = Helpers.CreateRandomPost(random);
            mockRepository.Setup(repository => repository.getPostAsync(post.Id)).ReturnsAsync(post);
            var conroller = new HomeController(mockRepository.Object, fileManager, mapper);

            var result = await conroller.Post(post.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            Assert.Equal(post.Id, model.Id);
        }

        [Fact]
        public async Task Comment_ReturnRedirectAndPost_WhenModdelStateIsValid()
        {
            var post = Helpers.CreateRandomPost(random);
            var comment = Helpers.CreateRandomComment(random);
            comment.PostId = post.Id;
            mockRepository.Setup(repository => repository.getPostAsync(post.Id)).ReturnsAsync(post);
            var moqMapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            var mapper = moqMapper.CreateMapper();
            var conroller = new HomeController(mockRepository.Object, fileManager, mapper);

            var result = await conroller.Comment(comment);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Post", redirectToActionResult.ActionName);
        }
    }
}
