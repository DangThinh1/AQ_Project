using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Core.Models
{
    public class SelectListModel
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public SelectListModel(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}
