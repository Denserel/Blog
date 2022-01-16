namespace Blog.Data
{
    public interface IRepository
    {
        List<Post> getPost(int? id);
        void deletePost(int id);
        void updatePost(Post post);
        void addPost(Post post);
        Task<bool> SaveChanges();
    }
}
