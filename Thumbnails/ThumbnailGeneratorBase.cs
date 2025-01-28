using Microsoft.Extensions.Configuration;

using Thumbnail.API.Models;

namespace Thumbnail.API.Thumbnails
{
    public abstract class ThumbnailGeneratorBase
    {
        protected string BinariesPath { get; set; }
        protected string ImageCachePath { get; set; }
        protected int ThumbnailSize { get; set; }
        protected string DefaultThumbnailFormat { get; set; }
        protected string VideoEnginePath { get; set; }

        protected string FullSourcePath { get; set; }
        protected string FullTargetThumbnailPath { get; set; }
        protected string OriginalFileName { get; set; }    
        protected string FileNameWithDefaultExtension { get; set; }

        protected List<string> WordFormats { get; set; }
        protected List<string> ExcelFormats { get; set; }
        protected List<string> PresentationFormats { get; set; }
        protected List<string> PdfFormats { get; set; }
        protected List<string> FullOfficeFormats { get; set; } = new List<string>();

        protected ThumbnailGeneratorBase(AppSettings appSettings, string fileName)
        {
            BinariesPath  = appSettings.BinariesPath;
            ImageCachePath  = appSettings.ImageCachePath;
            ThumbnailSize  = appSettings.ThumbnailSize;
            VideoEnginePath = appSettings.VideoEnginePath;
            DefaultThumbnailFormat = appSettings.DefaultThumbnailFormat;

            FullSourcePath = Path.Combine(BinariesPath, fileName);
            OriginalFileName = fileName.ToLower();
            FileNameWithDefaultExtension = GetWithDefaultExtension(fileName);
            FullTargetThumbnailPath = Path.Combine(ImageCachePath, FileNameWithDefaultExtension);

            WordFormats = appSettings.WordFormats;
            ExcelFormats = appSettings.ExcelFormats;
            PresentationFormats = appSettings.PresentationFormats;
            PdfFormats = appSettings.PdfFormats;

            FullOfficeFormats.AddRange(WordFormats);
            FullOfficeFormats.AddRange(ExcelFormats);
            FullOfficeFormats.AddRange(PresentationFormats);
            FullOfficeFormats.AddRange(PdfFormats);
        }

        protected string GetWithDefaultExtension(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}{DefaultThumbnailFormat}";
        }
    }
}
