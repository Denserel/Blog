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
    public class AdminControllerTest
    {
        private Mock<IFileManager> mockFileManager = new Mock<IFileManager>();
        private readonly Random random = new();
        private Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();

        [Fact]
        public async Task Index_OnGet_ReturnAllPosts()
        {
            var posts = new List<Post>() { Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random) };
            var searchString = string.Empty;
            mockRepository.Setup(repository => repository.getAllPostsAsync(searchString)).ReturnsAsync(posts);
            var conroller = new AdminController(mockRepository.Object, mockFileManager.Object, mockMapper.Object);

            var result = await conroller.Index(searchString);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Post>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Index_WithSearchString_ReturnPostContainingSearchString ()
        {
            var posts = new List<Post>() { Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random), Helpers.CreateRandomPost(random) };
            var searchString = posts[1].Title;
            mockRepository.Setup(repository => repository.getAllPostsAsync(searchString))
                .ReturnsAsync(posts.Where(post => post.Category.Contains(searchString)
                                || post.Tags.Contains(searchString)
                                || post.Title.Contains(searchString)).ToList());
            var conroller = new AdminController(mockRepository.Object, mockFileManager.Object, mockMapper.Object);

            var result = await conroller.Index(searchString);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Post>>(viewResult.ViewData.Model);
            Assert.Equal(posts[1].Id, model[0].Id);
            Assert.Equal(1, model.Count);
        }

        [Fact]
        public async Task Edit_IdIsNull_ReturnViewWithPostViewModel()
        {
            var controller = new AdminController(mockRepository.Object, mockFileManager.Object, mockMapper.Object);

            var result = await controller.Edit(id: null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_PrivideId_ReturnPostToEdit()
        {
            var post = Helpers.CreateRandomPost(random);
            mockRepository.Setup(repository => repository.getPostAsync(post.Id)).ReturnsAsync(post);
            var moqMapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            var mapper = moqMapper.CreateMapper();
            var conroller = new AdminController(mockRepository.Object, mockFileManager.Object, mapper);

            var result = await conroller.Edit(id: post.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
            Assert.Equal(post.Id, model.Id);
        }

        [Fact]
        public async Task Edit_WhenModelStateIsValid_RedirectToActionIndex()
        {
            var postViewModel = Helpers.CreateRandomPostViewModel(random);
            if (postViewModel.Id > 0)
            {
                mockRepository.Setup(repository => repository.updatePost(It.IsAny<Post>())).Verifiable();
            }
            else
            {
                 mockRepository.Setup(repository => repository.addPost(It.IsAny<Post>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            }
            //mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);
            var moqMapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            var mapper = moqMapper.CreateMapper();
            var conroller = new AdminController(mockRepository.Object, mockFileManager.Object, mapper);

            var result = await conroller.Edit(postViewModel);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Edit_WhenModelStateIsInValid_ReturnsViewResult()
        {
            var postViewModel = Helpers.CreateRandomPostViewModel(random);
            var conroller = new AdminController(mockRepository.Object, mockFileManager.Object, mockMapper.Object);
            conroller.ModelState.AddModelError("Title", "Required");

            var result = await conroller.Edit(postViewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Delete_PrivideId_RedirectToActionIndex()
        {
            var post = Helpers.CreateRandomPost(random);
            mockRepository.Setup(repository => repository.getPostAsync(post.Id)).ReturnsAsync(post);
            var controller = new AdminController(mockRepository.Object, mockFileManager.Object, mockMapper.Object);

            var result = await controller.Delete(id: post.Id);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }


}
