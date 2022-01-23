namespace Blog.Data
{
    public class Repository : IRepository
    {
        private readonly AppDbContext db;

        public Repository(AppDbContext db)
        {
            this.db = db;
        }

        public void addPost(Post post)
        {
            db.Posts.Add(post);
        }

        public void deletePost(int id)
        {
            db.Posts.Remove(getPost(id));
        }

        public Post getPost(int id)
        {
            return db.Posts.FirstOrDefault(post => post.Id == id); 
        }

        public void updatePost(Post post)
        {
            db.Posts.Update(post);
        }
        public List<Post> getAllPosts()
        {
            return db.Posts.ToList();
        }
        
        public async Task<bool> SaveChanges()
        {
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

    }
}
