namespace Blog.Data
{
    public interface IFileManager
    {
        FileStream GetImage(string image);
        Task<string> SaveImage(IFormFile image);
    }
}
