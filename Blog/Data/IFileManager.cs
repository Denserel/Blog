namespace Blog.Data
{
    public interface IFileManager
    {
        FileStream GetImage(string image);
        Task<string> SaveImageAsync(IFormFile image);
        void RemoveImage(string image);
    }
}
