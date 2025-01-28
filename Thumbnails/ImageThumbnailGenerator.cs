using ImageMagick;
using System.Net.NetworkInformation;

using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;

namespace Thumbnail.API.Thumbnails
{
    public class ImageThumbnailGenerator : ThumbnailGeneratorBase, IThumbnailGenerator
    {
        public ImageThumbnailGenerator(AppSettings appSettings, string fileName) : base(appSettings, fileName) { }

        public void Create()
        {
            using MagickImage sourceImage = new MagickImage(FullSourcePath);

            MagickGeometry size = new MagickGeometry();
            size.IgnoreAspectRatio = false;

            if (sourceImage.BaseHeight > sourceImage.BaseWidth)
            {
                size.Height = (uint)ThumbnailSize;
            }
            else if (sourceImage.BaseHeight < sourceImage.BaseWidth)
            {
                size.Width = (uint)ThumbnailSize;
            }
            else
            {
                size.Height = size.Width = (uint)ThumbnailSize;
                size.IgnoreAspectRatio = true;
            }

            sourceImage.Resize(size);
            sourceImage.Write(FullTargetThumbnailPath);
        }
    }
}
