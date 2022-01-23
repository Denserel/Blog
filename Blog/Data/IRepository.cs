namespace Blog.Data
{
    public interface IRepository
    {
        Post getPost(int id);
        List<Post> getAllPosts();
        List<Post> getAllPosts(string category);
        void deletePost(int id);
        void updatePost(Post post);
        void addPost(Post post);
        Task<bool> SaveChanges();
    }
}
