using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class UserAPI
    {
        public UserAPI()
        {

        }
        [Key]
        public int id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
