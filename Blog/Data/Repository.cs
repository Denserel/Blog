namespace Blog.Data
{
    public class Repository : IRepository
    {
        private readonly AppDbContext dataBase;

        public Repository(AppDbContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public void addPost(Post post)
        {
            dataBase.Posts.Add(post);
        }

        public void deletePost(int id)
        {
            dataBase.Posts.Remove(getPost(id));
        }

        public Post getPost(int id)
        {
            return dataBase.Posts.
                Include(post => post.Comments)
                .ThenInclude(comment => comment.SubComments)
                .FirstOrDefault(post => post.Id == id); 
        }

        public void updatePost(Post post)
        {
            dataBase.Posts.Update(post);
        }
        public List<Post> getAllPosts()
        {
            return dataBase.Posts.ToList();
        }
        public List<Post> getAllPosts(string category)
        {
            return dataBase.Posts
                .Where(post => post.Category.ToLower().Equals(category.ToLower()))
                .ToList();
        }
        public async Task<bool> SaveChangesAsync()
        {
            if (await dataBase.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

    }
}
