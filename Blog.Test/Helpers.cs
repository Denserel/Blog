using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test;

public static class Helpers
{
    public static Post CreateRandomPost(Random random)
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

    public static CommentViewModel CreateRandomComment(Random random)
    {
        return new CommentViewModel()
        {
            PostId = random.Next(),
            MainCommentId = null,
            Message = Guid.NewGuid().ToString(),
            User = Guid.NewGuid().ToString()
        };
    }

    public static PostViewModel CreateRandomPostViewModel(Random random)
    {
        return new PostViewModel()
        {
            Id = random.Next(999),
            Title = Guid.NewGuid().ToString(),
            Body = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Tags = Guid.NewGuid().ToString(),
            Category = Guid.NewGuid().ToString(),
            CurrentImage = Guid.NewGuid().ToString(),
            Image = null
        };
    }
}