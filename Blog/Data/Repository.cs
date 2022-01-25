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

        public async Task deletePostAsync(int id)
        {
            dataBase.Posts.Remove(await getPostAsync(id));
        }

        public async Task <Post> getPostAsync(int id)
        {
            var post = await dataBase.Posts
                .Include(post => post.Comments)
                .ThenInclude(comment => comment.SubComments)
                .FirstOrDefaultAsync(post => post.Id == id);

            return post;
        }

        public void updatePost(Post post)
        {
            dataBase.Posts.Update(post);
        }
        public List<Post> getAllPosts()
        {
            return dataBase.Posts.ToList();
        }
        public async Task <List<Post>> getAllPostsAsync(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return await dataBase.Posts
                    .Where(post => post.Category.ToLower().Contains(searchString.ToLower())
                    || post.Tags.ToLower().Contains(searchString.ToLower())
                    || post.Title.ToLower().Contains(searchString.ToLower()))
                    .ToListAsync();
            }

            return await dataBase.Posts.ToListAsync();
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
