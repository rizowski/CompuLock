using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CompuWatch
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            ServiceProcessInstaller spinstaller = new ServiceProcessInstaller();
            ServiceInstaller sinstall = new ServiceInstaller();

            spinstaller.Account =ServiceAccount.LocalSystem;
            spinstaller.Username = null;
            spinstaller.Password = null;

            sinstall.DisplayName = "CompuLock Service";
            sinstall.Description = "CompuLock Monitoring Service";
            sinstall.StartType = ServiceStartMode.Automatic;
            sinstall.ServiceName = "CompuLock Services";

            this.Installers.Add(sinstall);
            this.Installers.Add(spinstaller);
            /*InitializeComponent();*/

        }
    }
}
