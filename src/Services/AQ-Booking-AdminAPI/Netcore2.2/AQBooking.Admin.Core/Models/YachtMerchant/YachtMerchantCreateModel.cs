﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchant
{
    public class YachtMerchantCreateModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int ZoneFid { get; set; }
        public string MerchantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        public int AccountSize { get; set; }
        public string Remark { get; set; }
        public DateTime ExpiredDate { get; set; }

        public YachtMerchantCreateModel()
        {
            ExpiredDate = DateTime.Now.AddYears(1);
        }
    }
}
