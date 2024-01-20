using System.IO;
using System.Threading.Tasks;
namespace BootcampAPI.Service
{
    public class LocalService : ILocalService
    {
        private readonly string _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");


        public async Task<bool> DeleteBlob(string filename)
        {
            var filePath = Path.Combine(_uploadsPath, filename);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true; // File deleted successfully
                }
                else
                {
                    return false; // File not found
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<string> UploadBlob(string filename, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_uploadsPath, filename);

                // Ensure directory exists
                Directory.CreateDirectory(_uploadsPath);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return filePath; // Return local file path
                }
                catch (Exception ex)
                {
                    // Handle errors gracefully
                    throw; // Re-throw to allow for higher-level handling
                }
            }

            return ""; // Handle invalid file
        }
    }
}