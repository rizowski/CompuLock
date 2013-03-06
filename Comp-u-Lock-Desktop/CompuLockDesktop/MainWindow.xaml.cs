using System;
using System.Collections.ObjectModel;
using System.Windows;
using Database.Models;
using Service;

namespace CompuLockDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainService service;
        public MainWindow()
        {
            InitializeComponent();
            service = new MainService();
            LoadAccounts();
            LoadOverviewccounts();
            LoadComputer();
            LoadHistory();
            LoadProcesses();
        }

        private void LoadAccounts()
        {
            var accounts = service.GetAccounts();
            Accounts.ItemsSource = accounts;
        }

        private void OnOpen(object sender, EventArgs e)
        {
            //I don't think this is a good idea any more.
            /*try
            {
                service.GetDbUser();
            }
            catch (NullReferenceException ex)
            {
                var dialog = new Options(service);
                dialog.Show();
                MessageBox.Show(ex.Message);
            }*/
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Options(service);
            dialog.ShowDialog();
        }

        private void LoadOverviewccounts()
        {
            var dbaccounts = service.GetDbAccounts();
            if(dbaccounts!= null)
            foreach (var dbaccount in dbaccounts)
            {
                OverviewAccounts.Items.Add(dbaccount.Username);
            }
        }

        private void LoadHistory()
        {
            var histories = service.GetHistory();
            if (histories != null)
                Histories.ItemsSource = histories;
        }

        private void LoadComputer()
        {
            var computer = service.GetDbComputer();
            if (computer != null)
            {
                ComputerName.Content = computer.Name;
                Enviroment.Content = computer.Enviroment;
            }
        }

        private void LoadProcesses()
        {
            var processes = service.GetProcesses();
            if (processes != null)
                Processes.ItemsSource = processes;
        }

        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            service.ProcessManager.Dispose();
        }

    }
}
