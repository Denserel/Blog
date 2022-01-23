namespace Blog.Data
{
    public interface IRepository
    {
        Post getPost(int id);
        List<Post> getAllPosts();
        void deletePost(int id);
        void updatePost(Post post);
        void addPost(Post post);
        Task<bool> SaveChanges();
    }
}
