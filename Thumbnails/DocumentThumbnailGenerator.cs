using System.Drawing;

using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;

using PdfLibCore;
using PdfLibCore.Enums;

using Spire.Doc;
using Spire.Presentation;
using Spire.Xls;

namespace Thumbnail.API.Thumbnails
{
    public class DocumentThumbnailGenerator : ThumbnailGeneratorBase, IThumbnailGenerator
    {
        public DocumentThumbnailGenerator(AppSettings appSettings, string fileName) : base(appSettings, fileName) { }

        public void Create() 
        {
            if (WordFormats.Contains(Path.GetExtension(OriginalFileName)))
            {
                GenerateWordThumbnail(FullSourcePath, FullTargetThumbnailPath);
            }
            else if (ExcelFormats.Contains(Path.GetExtension(OriginalFileName)))
            {
                GenerateExcelThumbnail(FullSourcePath, FullTargetThumbnailPath);
            }
            else if (PresentationFormats.Contains(Path.GetExtension(OriginalFileName)))
            {
                GeneratePowerpointhumbnail(FullSourcePath, FullTargetThumbnailPath);
            }
            else if (PdfFormats.Contains(Path.GetExtension(OriginalFileName)))
            {
                GeneratePdfhumbnail(FullSourcePath, FullTargetThumbnailPath);
            }
        }

        private void GenerateWordThumbnail(string sourceToUpload, string thumbnailDestination)
        {
            Document document = new Document();
            document.LoadFromFile(sourceToUpload);
            Image image = document.SaveToImages(0, Spire.Doc.Documents.ImageType.Bitmap);
            Image resized = ResizeImage(image, ThumbnailSize);
            resized.Save(thumbnailDestination);
        }

        private void GenerateExcelThumbnail(string sourceToUpload, string thumbnailDestination)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(sourceToUpload);
            Worksheet sheet = workbook.Worksheets[0];
            Image image = sheet.ToImage(1, 1, sheet.LastRow, sheet.LastColumn);
            Image resized = ResizeImage(image, ThumbnailSize);
            resized.Save(thumbnailDestination);
        }

        private void GeneratePowerpointhumbnail(string sourceToUpload, string thumbnailDestination)
        {
            Presentation presentation = new Presentation();
            presentation.LoadFromFile(sourceToUpload);
            Image image = presentation.Slides[0].SaveAsImage();
            Image resized = ResizeImage(image, ThumbnailSize);
            resized.Save(thumbnailDestination);
        }

        private void GeneratePdfhumbnail(string sourceToUpload, string thumbnailDestination)
        {
            using PdfDocument pdfDocument = new PdfDocument(sourceToUpload);
            PdfPage pdfPage = pdfDocument.Pages[0];
            double xScale = ThumbnailSize / pdfPage.Size.Width;
            double yScale = ThumbnailSize / pdfPage.Size.Height;
            double scale = Math.Min(xScale, yScale);
            using PdfiumBitmap bitmap = new PdfiumBitmap((int)(scale * pdfPage.Size.Width), (int)(scale * pdfPage.Size.Height), true);
            pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
            Stream bmpStream = bitmap.AsBmpStream();

            using FileStream destinationStream = new FileStream(thumbnailDestination, FileMode.Create, FileAccess.Write);
            bmpStream.CopyTo(destinationStream);
        }

        private static Image ResizeImage(Image originalImage, int thumbnailSize)
        {
            int originalHeight = originalImage.Height;
            int originalWidth = originalImage.Width;
            
            int resizedHeight = 0;
            int resizedWidth = 0;

            if (originalHeight > originalWidth)
            {
                resizedHeight = thumbnailSize;
                resizedWidth = (int)Math.Round(((double)originalWidth / originalHeight) * resizedHeight);
            }
            else if (originalHeight < originalWidth)
            {
                resizedWidth = thumbnailSize;
                resizedHeight = (int)Math.Round(((double)originalHeight /originalWidth) * resizedWidth);
            }
            else
            {
                resizedHeight = resizedWidth = thumbnailSize;
            }

            Bitmap resizedImage = new Bitmap(resizedWidth, resizedHeight);

            using Graphics graphics = Graphics.FromImage(resizedImage);

            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.DrawImage(originalImage, 0, 0, resizedWidth, resizedHeight);

            return resizedImage;
        }
    }
}
