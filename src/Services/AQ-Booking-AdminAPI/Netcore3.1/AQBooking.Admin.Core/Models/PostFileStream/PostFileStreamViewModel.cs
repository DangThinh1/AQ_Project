using System;

namespace AQBooking.Admin.Core.Models.PostFileStream
{
    public class PostFileStreamViewModel
    {
        public long PostFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public int FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}
