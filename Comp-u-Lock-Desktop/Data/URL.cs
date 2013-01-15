using System;

namespace Data
{
    public class URL
    {
        public string Url;
        public string Title;
        public string Browser;
        public DateTime LastVisitDate;
        public int VisitCount;

        public URL(long lastvisitdate, string url, string title, string browser, int visitcount = 0)
        {
            Url = url;
            Title = title;
            Browser = browser;           
            LastVisitDate = new DateTime(lastvisitdate);
            VisitCount = visitcount;

        }

        public override string ToString()
        {
            return Browser + " - " + Title + " - " + Url;
        }


    }
}
