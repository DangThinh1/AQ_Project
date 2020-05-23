using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Databases.YachtEntities
{
    public partial class YachtTourFileStreams
    {
        public long Id { get; set; }
        public int YachtTourFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileTypeFid { get; set; }
        public long FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
