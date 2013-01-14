using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class URL
    {
        public string Url;
        public string Title;
        public string Browser;

        public URL(string url, string title, string browser)
        {
            Url = url;
            Title = title;
            Browser = browser;
        }

        public new string ToString()
        {
            return Browser + " - " + Title + " - " + Url;
        }

    }
}
