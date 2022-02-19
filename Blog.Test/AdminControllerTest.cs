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
        private IFileManager fileManager;
        private readonly Random random = new();
        private Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();

        [Fact]
        public async Task Index_OnGetCall_ReturnAllPosts()
        {
            var posts = new List<Post>() { CreateRandomPost(), CreateRandomPost(), CreateRandomPost() };
            var searchString = string.Empty;
            mockRepository.Setup(repository => repository.getAllPostsAsync(searchString)).ReturnsAsync(posts);
            var conroller = new AdminController(mockRepository.Object, fileManager, mockMapper.Object);

            var result = await conroller.Index(searchString);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Post>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Edit_IdIsNull_ReturnViewWithPostViewModel()
        {
            var controller = new AdminController(mockRepository.Object, fileManager, mockMapper.Object);

            var result = await controller.Edit(id: null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_PrivideId_ReturnPostToEdit()
        {
            var post = CreateRandomPost();
            mockRepository.Setup(repository => repository.getPostAsync(post.Id)).ReturnsAsync(post);
            var moqMapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            var mapper = moqMapper.CreateMapper();
            var conroller = new AdminController(mockRepository.Object, fileManager, mapper);

            var result = await conroller.Edit(id: post.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
            Assert.Equal(post.Id, model.Id);
        }

        private Post CreateRandomPost()
        {
            return new Post()
            {
                Id = random.Next(999),
                Title = Guid.NewGuid().ToString(),
                Body = Guid.NewGuid().ToString(),
                Image = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Tags = Guid.NewGuid().ToString(),
                Category = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Comments = new List<MainComment> { }
            };
        }

        private CommentViewModel CreateRandomComment()
        {
            return new CommentViewModel()
            {
                PostId = random.Next(),
                MainCommentId = null,
                Message = Guid.NewGuid().ToString(),
                User = Guid.NewGuid().ToString()
            };
        }
    }


}
