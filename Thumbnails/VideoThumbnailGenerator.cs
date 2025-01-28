using System.Drawing;

using FFMpegCore;

using Thumbnail.API.Interfaces;
using Thumbnail.API.Models;

namespace Thumbnail.API.Thumbnails
{
    public class VideoThumbnailGenerator : ThumbnailGeneratorBase, IThumbnailGenerator
    {
        public VideoThumbnailGenerator(AppSettings appSettings, string fileName) : base(appSettings, fileName) { }
        public void Create() 
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder = VideoEnginePath);
            Size size = new Size();

            IMediaAnalysis fileInfo = FFProbe.Analyse(FullSourcePath);

            int width = fileInfo.PrimaryVideoStream != null ? fileInfo.PrimaryVideoStream.Width : 0;
            int height = fileInfo.PrimaryVideoStream != null ? fileInfo.PrimaryVideoStream.Height : 0;

            if (width > height)
            {
                size.Width = ThumbnailSize;
            }
            else if (width < height)
            {
                size.Height = ThumbnailSize;
            }
            else
            {
                size.Height = size.Width = ThumbnailSize;
            }

            Snapshot(FullSourcePath, FullTargetThumbnailPath, size, TimeSpan.FromSeconds(3), fileInfo);
        }

        private void Snapshot(string input, string output, Size size, TimeSpan captureTime, IMediaAnalysis source)
        {
            (FFMpegArguments fFMpegArguments, Action<FFMpegArgumentOptions> addArguments) = SnapshotArgumentBuilder.BuildSnapshotArguments(input, source, size, captureTime, null, 0);
            fFMpegArguments.OutputToFile(output, overwrite: true, addArguments).ProcessSynchronously();
        }
    }
}