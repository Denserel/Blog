
using System.Drawing;
using System.Drawing.Imaging;

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
            return new FileStream(Path.Combine(imagePath, image), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync:true);
        }


        public async Task<string> SaveImageAsync(IFormFile file)
        {
            try
            {
                var path = Path.Combine(imagePath);
               
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                var fileName = Guid.NewGuid().ToString()+file.FileName;

                // Resize Image
                var image = Image.FromStream(file.OpenReadStream());
                var resized = new Bitmap(image, new Size(400, 400));
                using var imageSteam = new MemoryStream();
                resized.Save(imageSteam, ImageFormat.Png);
                var imageBytes = imageSteam.ToArray();
                

                using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
                {
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
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
