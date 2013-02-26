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
    public partial class MainWindow : Window
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

        private void LoadComputer()
        {
            var computer = service.GetDbComputer();
            ComputerName.Content = computer.Name;
            
        }

    }
}
