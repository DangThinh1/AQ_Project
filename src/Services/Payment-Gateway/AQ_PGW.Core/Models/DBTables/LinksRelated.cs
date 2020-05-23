using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class LinksRelated
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
        public long IdRelatedTransaction { get; set; }

    }
}
