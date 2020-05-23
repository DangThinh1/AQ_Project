namespace AQBooking.FileStreamAPI.Services.Media
{
    /// <summary>
    /// Represents default values related to media services
    /// </summary>
    public static partial class AQMediaDefaults
    {
        /// <summary>
        /// Gets a key to cache whether thumb exists
        /// </summary>
        /// <remarks>
        /// {0} : thumb file name
        /// </remarks>
        public static string ThumbExistsCacheKey => "";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ThumbsPatternCacheKey => "";

        /// <summary>
        /// Gets a multiple thumb directories length
        /// </summary>
        public static int MultipleThumbDirectoriesLength => 3;

        /// <summary>
        /// Gets a path to the image thumbs files
        /// </summary>
        public static string ImageThumbsPath => @"contents\images\thumbs";

        /// <summary>
        /// Gets a path to the default image files
        /// </summary>
        public static string DefaultImagePath => "contents/images/";

        /// <summary>
        /// Gets a default avatar file name
        /// </summary>
        public static string DefaultAvatarFileName => "default-avatar.jpg";

        /// <summary>
        /// Gets a default image file name
        /// </summary>
        public static string DefaultImageFileName => "default-image.png";
    }
}