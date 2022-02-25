namespace Blog.Data
{
    public class Repository : IRepository
    {
        private readonly AppDbContext dataBase;

        public Repository(AppDbContext dataBase)
        {
            this.dataBase = dataBase;
        }
        public async Task addPost(Post post)
        {
             await dataBase.Posts.AddAsync(post);
        }
        public async Task deletePostAsync(int id)
        {
            dataBase.Posts.Remove(await getPostAsync(id));
        }
        public async Task<Post> getPostAsync(int id)
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
        
        public async Task<List<Post>> getAllPostsAsync(string searchString = "")
        {
            //searchString ??= "";

            var posts = from post in dataBase.Posts
                        select post;

            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(post => post.Category.Contains(searchString) 
                                    || post.Tags.Contains(searchString)
                                    || post.Title.Contains(searchString));
            }
            
            return await posts.ToListAsync();
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
