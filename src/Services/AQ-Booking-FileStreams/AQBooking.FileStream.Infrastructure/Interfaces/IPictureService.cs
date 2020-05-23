using AQBooking.FileStream.Core.Enums;
using AQBooking.FileStream.Core.Models.Picture;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;

namespace AQBooking.FileStream.Infrastructure.Interfaces
{
    public interface IPictureService
    {
        byte[] EncodeImage<T>(Image<T> image, IImageFormat imageFormat, int? quality = null) where T : struct, IPixel<T>;
        string GetFileExtensionFromMimeType(string mimeType);
        ImageFormatEnum GetImageFormat(byte[] bytes);
        string GetPictureLocalPath(string fileName);
        string GetPictureLocalPath(string fileName, string folderName);
        string GetThumbLocalPath(string thumbFileName);
        bool IsImageFile(IFormFile file);
        byte[] LoadPictureFromFile(int pictureId, string mimeType);
        void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType);
        void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary);
        bool isPortraitImage(byte[] pictureBinary);
        byte[] ValidatePicture(byte[] pictureBinary, int maximumImageSize, int quality);
        byte[] ResizePicture(byte[] pictureBinary, int ratio, int quality);
    }
}
