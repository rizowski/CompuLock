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
        public int LastVisitDate;
        public int VisitCount;

        public URL(int lastvisitdate, string url, string title, string browser, int visitcount = 0)
        {
            Url = url;
            Title = title;
            Browser = browser;
            LastVisitDate = lastvisitdate;
            VisitCount = visitcount;

        }

        public new string ToString()
        {
            return Browser + " - " + Title + " - " + Url;
        }

    }
}
