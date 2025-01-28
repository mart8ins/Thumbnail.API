using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;
using Thumbnail.API.Thumbnails;

namespace Thumbnail.API.Services
{
    public class ThumbnailService: IThumbnailService
    {
        private readonly IRepositoryService _repository;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ThumbnailService> _logger;
        private long Progress { get; set; } = 0;
        private long TotalCount { get; set; }

        public ThumbnailService(IRepositoryService repository, AppSettings appSettings, ILogger<ThumbnailService> logger)
        {
            _repository = repository;
            _appSettings = appSettings;
            _logger = logger;
        }

        public void ProcessFiles()
        {
            long offset = 0;
            long chunkSize = 500;

            TotalCount = _repository.GetTotalFileCount();

            _logger.LogInformation($"Total of files for thumbnail creation {TotalCount}. Processing in chunks, {chunkSize} chunk size.");

            IEnumerable<string> fileNames = _repository.GetFileNames(offset, chunkSize);

            while (fileNames != null && fileNames.Any())
            {
                _logger.LogInformation($"Processing chunk size {chunkSize}/{fileNames.Count()}");

                foreach (string fileName in fileNames)
                {
                    Progress++;
                    GenerateThumbnail(fileName);
                    _logger.LogInformation($"{Progress}/{TotalCount} Thumbnail for {fileName} file created.");
                }

                offset += chunkSize;
                fileNames = _repository.GetFileNames(offset, chunkSize);
            }

            _logger.LogInformation($"Thumbnail creation for full database completed.");
        }

        public void ProcessFile(string fileId)
        {
            string fileName = _repository.GetFileName(fileId);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                GenerateThumbnail(fileName);
                _logger.LogInformation($"Thumbnail for {fileName} file created.");
            }
            else
            {
                _logger.LogWarning($"Can`t find file name for file with id: {fileId}.");
            }
        }

        private void GenerateThumbnail(string fileName)
        {
            try
            {
                IThumbnailGenerator thumbnailGenerator = new ThumbnailFactory(_appSettings).CreateThumbnailGenerator(fileName);
                thumbnailGenerator.Create();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Progress}/{TotalCount} Thumbnail creation for {fileName} failed.");
                _logger.LogError(ex.Message);
            }
        }
    }
}
