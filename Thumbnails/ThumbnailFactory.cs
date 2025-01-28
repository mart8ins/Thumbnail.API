using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;

namespace Thumbnail.API.Thumbnails
{
    public class ThumbnailFactory
    {
        private readonly AppSettings _appSettings;

        public ThumbnailFactory(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IThumbnailGenerator CreateThumbnailGenerator(string fileName) 
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();

            if (_appSettings.ImageFormats.Contains(fileExtension))
            {
                return new ImageThumbnailGenerator(_appSettings, fileName);
            } 
            else if (_appSettings.VideoFormats.Contains(fileExtension))
            {
                return new VideoThumbnailGenerator(_appSettings, fileName);
            }
            else if (_appSettings.FullOfficeFormats.Contains(fileExtension))
            {
                return new DocumentThumbnailGenerator(_appSettings, fileName);
            }

            throw new NotImplementedException($"Provided file with extension '{fileExtension}' is not configured to be processed for thumbnail.");
        }
    }
}
