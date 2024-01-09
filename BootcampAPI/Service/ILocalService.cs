namespace BootcampAPI.Service
{
    public interface ILocalService
    {
          Task<string> UploadBlob(string filename, IFormFile file);
         Task<bool> DeleteBlob(string fileName);
    }
}
