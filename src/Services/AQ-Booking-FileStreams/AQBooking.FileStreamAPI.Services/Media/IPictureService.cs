using AQBooking.FileStreamAPI.Core.Domain.Media;
using AQBooking.FileStreamAPI.Core.Enums;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace AQBooking.FileStreamAPI.Services.Media
{
    public interface IPictureService
    {
        Size CalculateDimensions(Size originalSize, int targetSize, ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true);
        void DeletePictureOnFileSystem(Picture picture);
        byte[] EncodeImage<T>(Image<T> image, IImageFormat imageFormat, int? quality = null) where T : struct, IPixel<T>;
        string GetDefaultPictureUrl(int targetSize = 0, PictureType defaultPictureType = PictureType.Entity, string storeLocation = null);
        string GetFileExtensionFromMimeType(string mimeType);
        ImageFormatEnum GetImageFormat(byte[] bytes);
        string GetPictureLocalPath(string fileName);
        string GetPictureLocalPath(string fileName, string folderName);
        string GetThumbLocalPath(string thumbFileName);
        string GetThumbUrl(string thumbFileName, string storeLocation = null);
        bool IsImageFile(IFormFile file);
        byte[] LoadPictureFromFile(int pictureId, string mimeType);
        void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType);
        void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary);
        bool isPortraitImage(byte[] pictureBinary);
        byte[] ValidatePicture(byte[] pictureBinary, int maximumImageSize, int quality);
        byte[] ResizePicture(byte[] pictureBinary, int ratio, int quality);
    }
}
