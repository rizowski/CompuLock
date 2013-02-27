using System;
using System.Collections.Generic;
using System.Linq;
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
            LoadComputer();
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

        private void LoadAccounts()
        {
            var dbaccounts = service.GetDbAccounts();
            foreach (var dbaccount in dbaccounts)
            {
                Accounts.Items.Add(dbaccount.Username);
            }
        }

        private void LoadHistory()
        {
            var histories = service.GetHistory();
            foreach (var history in histories)
            {
                Histories.Items.Add(history);
            }
        }

        private void LoadComputer()
        {
            var computer = service.GetDbComputer();
            ComputerName.Content = computer.Name;
            Enviroment.Content = computer.Enviroment;
        }

        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            service.ProcessManager.Dispose();
        }

        private void OnHistoryTabClick(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            LoadHistory();
        }

    }
}
