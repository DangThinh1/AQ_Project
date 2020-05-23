using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Common.FileStream
{
    public class FileDataUploadRequestModel
    {
        /// <summary>
        /// The file name is the original name of the selected file
        /// </summary>
        public string FileName { get; set; }
        public string FileExtention { get; set; }
        /// <summary>
        /// File data is byte array
        /// </summary>
        public string FileData { get; set; }
        /// <summary>
        /// File type id taken from the FileTypeEnum
        /// </summary>
        public int FileTypeFid { get; set; }
        /// <summary>
        /// DomainId taken from the uniqueId of domain portal in "CommonValues" table, ex: EHDY67UHNYJN
        /// </summary>
        public string DomainId { get; set; }
        /// <summary>
        /// FolderId taken from the uniqueId of the yacht..., ex: UTHJNGUTJ5FG
        /// </summary>
        public string FolderId { get; set; }
    }
}
