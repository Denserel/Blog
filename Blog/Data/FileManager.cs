namespace Blog.Data
{
    public class FileManager : IFileManager
    {
        private string imagePath;
        private readonly IWebHostEnvironment hostEnvironment;

        public FileManager(IWebHostEnvironment hostEnvironment)
        {
            imagePath = hostEnvironment.WebRootPath + "/img/blog";
            this.hostEnvironment = hostEnvironment;
        }

        public FileStream GetImage(string image)
        {
            return new FileStream(Path.Combine(imagePath, image), FileMode.Open, FileAccess.Read);
        }


        public async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                var path = Path.Combine(imagePath);
               
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                
                var fileName = Guid.NewGuid().ToString()+image.FileName;

                using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }
        public void RemoveImage(string image)
        {
            var file = Path.Combine(imagePath, image);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}
