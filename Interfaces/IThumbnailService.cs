namespace Thumbnail.API.Interfaces
{
    public interface IThumbnailService
    {
        public void ProcessFiles();
        public void ProcessFile(string fileId);
    }
}
