namespace Blog.Data
{
    public interface IRepository
    {
        Task <Post> getPostAsync(int id);
        List<Post> getAllPosts();
        Task <IQueryable<Post>> getAllPostsAsync(string searchString);
        Task deletePostAsync(int id);
        void updatePost(Post post);
        Task addPost(Post post);
        Task<bool> SaveChangesAsync();
    }
}
