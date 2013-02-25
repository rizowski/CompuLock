using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CompuWatch
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost host;
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "CompuLock";
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;

            Uri baseAddress = new Uri("http://localhost:8090/hello");

            host = new ServiceHost(typeof (Contracts), baseAddress);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.WriteEntry("Started");
            Console.WriteLine("Started");
            host.Open();
        }

        protected override void OnStop()
        {
            this.EventLog.WriteEntry("Ended");
            Console.WriteLine("Stopped");
            host.Close();
        }
    }
}
