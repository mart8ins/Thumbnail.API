namespace Thumbnail.API.Interfaces
{
    public interface IRepositoryService
    {
        IEnumerable<string> GetFileNames(long offset, long chunkSize);
        string GetFileName(string fileId);
        long GetTotalFileCount();
    }
}
