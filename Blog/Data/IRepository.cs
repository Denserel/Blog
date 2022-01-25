namespace Blog.Data
{
    public interface IRepository
    {
        Task <Post> getPostAsync(int id);
        List<Post> getAllPosts();
        Task <List<Post>> getAllPostsAsync(string searchString);
        Task deletePostAsync(int id);
        void updatePost(Post post);
        void addPost(Post post);
        Task<bool> SaveChangesAsync();
    }
}
