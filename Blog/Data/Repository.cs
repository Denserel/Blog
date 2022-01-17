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
            db.Posts.Remove(getPost(id).FirstOrDefault());
        }

        public List<Post> getPost(int? id)
        {
            List<Post> posts = new();

            if (id != null)
            {
                var result = db.Posts.FirstOrDefault(post => post.Id == id);

                posts.Add(result ?? new Post());
            }else
            {
                posts = db.Posts.ToList();
            }

            return posts;
        }

        public void updatePost(Post post)
        {
            db.Posts.Update(post);
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
