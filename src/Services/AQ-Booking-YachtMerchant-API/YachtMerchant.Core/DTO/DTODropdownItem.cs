using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.DTO
{
    public class DTODropdownItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public DTODropdownItem(string value, string text)
        {
            Value = value;
            Text = text;
        }
        public DTODropdownItem()
        {


        }

        
    }


}
