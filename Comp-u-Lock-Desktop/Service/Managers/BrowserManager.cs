using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Profile;

namespace Service.Managers
{
    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<URL> GetHistory();

    }

    public abstract class BrowserManager : IBrowser
    {
        protected const string Endline=";";

        public abstract bool IsRunning();
        public abstract IEnumerable<URL> GetHistory();
    }
}
