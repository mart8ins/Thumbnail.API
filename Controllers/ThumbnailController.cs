using Hangfire;

using Microsoft.AspNetCore.Mvc;

using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;

namespace Thumbnail.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ThumbnailController : ControllerBase
    {
        private readonly ILogger<ThumbnailController> _logger;
        private readonly IThumbnailService _thumbnailService;
        private readonly AppSettings _appSettings;

        public ThumbnailController(IThumbnailService thumbnailService, ILogger<ThumbnailController> logger, AppSettings appSettings)
        {
            _thumbnailService = thumbnailService;
            _logger = logger;
            _appSettings = appSettings;
        }

        [HttpPost("create")]
        public IActionResult GenerateForOne(string fileId)
        {
            _logger.LogInformation($"Request for thumbnail generation for file with id: {fileId}.");
            _logger.LogInformation($"Thumbnail size: {_appSettings.ThumbnailSize}. Default format: {_appSettings.DefaultThumbnailFormat}");
            _logger.LogInformation($"File source path: {_appSettings.BinariesPath}");
            _logger.LogInformation($"Thumbnail destination path: {_appSettings.ImageCachePath}");

            BackgroundJob.Enqueue(() => _thumbnailService.ProcessFile(fileId));

            return Accepted();
        }

        [HttpGet("create-all")]
        public IActionResult GenerateForAll()
        {
            _logger.LogInformation($"Request for all database thumbnail generation.");
            _logger.LogInformation($"Thumbnail size: {_appSettings.ThumbnailSize}. Default format: {_appSettings.DefaultThumbnailFormat}");
            _logger.LogInformation($"File source path: {_appSettings.BinariesPath}");
            _logger.LogInformation($"Thumbnail destination path: {_appSettings.ImageCachePath}");

            BackgroundJob.Enqueue(() => _thumbnailService.ProcessFiles());

            return Accepted();
        }
    }
}
