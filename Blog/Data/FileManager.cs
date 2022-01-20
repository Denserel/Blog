namespace Blog.Data
{
    public class FileManager : IFileManager
    {
        private string imagePath;

        public FileManager(IConfiguration configuration)
        {
            imagePath = configuration["Path:Images"];
        }

        public FileStream GetImage(string image)
        {
            return new FileStream(Path.Combine(imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var path = Path.Combine(imagePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}{mime}";

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
    }
}
