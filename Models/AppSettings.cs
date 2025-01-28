namespace Thumbnail.API.Models
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;

        public required string BinariesPath { get; set; }
        public required string ImageCachePath { get; set; }
        public required string VideoEnginePath { get; set; }
        public required int ThumbnailSize { get; set; }
        public required string DefaultThumbnailFormat { get; set; }
        public required List<string> ImageFormats { get; set; }
        public required List<string> VideoFormats { get; set; }
        public required List<string> WordFormats { get; set; }
        public required List<string> ExcelFormats { get; set; }
        public required List<string> PresentationFormats { get; set; }
        public required List<string> PdfFormats { get; set; }
        public required List<string> FullOfficeFormats { get; set; } = new List<string>();

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            InitAppSettings();
        }

        private void InitAppSettings()
        {
            BinariesPath = _configuration["BinariesPath"]!;
            ImageCachePath = _configuration["ImageCachePath"]!;
            VideoEnginePath = _configuration["VideoEnginePath"]!;
            ThumbnailSize = int.TryParse(_configuration["ThumbnailSize"], out int parsedSize) ? parsedSize : 240;
            DefaultThumbnailFormat = _configuration["DefaultThumbnailFormat"] ?? ".jpg";

            ImageFormats = _configuration.GetSection("ImageFormats").Get<List<string>>() ?? new List<string>();
            VideoFormats = _configuration.GetSection("VideoFormats").Get<List<string>>() ?? new List<string>();

            WordFormats = _configuration.GetSection("WordFormats").Get<List<string>>() ?? new List<string>();
            ExcelFormats = _configuration.GetSection("ExcelFormats").Get<List<string>>() ?? new List<string>();
            PresentationFormats = _configuration.GetSection("PresentationFormats").Get<List<string>>() ?? new List<string>();
            PdfFormats = _configuration.GetSection("PdfFormats").Get<List<string>>() ?? new List<string>();

            FullOfficeFormats.AddRange(WordFormats);
            FullOfficeFormats.AddRange(ExcelFormats);
            FullOfficeFormats.AddRange(PresentationFormats);
            FullOfficeFormats.AddRange(PdfFormats);
        }
    }
}
