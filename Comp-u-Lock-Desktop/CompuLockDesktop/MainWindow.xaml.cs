using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Database.Models;
using Service;
using Timer = System.Timers.Timer;

namespace CompuLockDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainService service;

        private Timer time;

        public MainWindow()
        {
            InitializeComponent();
            service = new MainService();
            time= new Timer();
            time.Interval = 3 * 1000;
            time.Elapsed += Update;
            time.Start();
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) delegate()
                {
                    LoadAccounts();
                    LoadComputer();
                    LoadHistory();
                    LoadProcesses();
                });
        }

        private void OnOpen(object sender, EventArgs e)
        {
            LoadAccounts();
            LoadComputer();
            LoadHistory();
            LoadProcesses();
            LoadUpdate();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Options(service);
            dialog.ShowDialog();
        }

        private void LoadAccounts()
        {
            var dbaccounts = service.GetDbAccounts();
            if (dbaccounts != null)
            {
                Accounts.ItemsSource = dbaccounts;
                OverviewAccounts.ItemsSource = dbaccounts;
            }
        }

        private void LoadUpdate()
        {
            var dbaccounts = service.GetDbAccounts();
            if (dbaccounts != null)
            {
                SelectAccount.ItemsSource = dbaccounts;
            }
        }

        private void LoadHistory()
        {
            Histories.ItemsSource = null;
            var histories = service.GetHistory();
            if (histories != null)
            {
                Histories.ItemsSource = histories;
                OverviewHistories.ItemsSource = histories;
            }
        }

        private void LoadComputer()
        {
            var computer = service.GetDbComputer();
            if (computer != null)
            {
                ComputerName.Content = computer.Name;
                Enviroment.Content = computer.Enviroment;
                IpAddress.Content = computer.IpAddress;
            }
        }

        private void LoadProcesses()
        {
            Processes.ItemsSource = null;
            var processes = service.GetProcesses();
            if (processes != null)
            {
                Processes.ItemsSource = processes;
                OverviewProcesses.ItemsSource = processes;
            }
        }

        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectAccount.SelectedValue != null)
            {
                var id = (int)SelectAccount.SelectedValue;
                var account = service.GetAccountById(id);
                var hours = Convert.ToInt32(Hours.Text);
                var minutes = Convert.ToInt32(Minutes.Text);
                var seconds = (hours*60*60) + minutes*60;
                account.AllottedTime = seconds;
                account.Tracking = (bool) Tracking.IsChecked;
                service.UpdateAccount(account);
            }
        }

        private void OnChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectAccount.SelectedValue != null)
            {
                var id = (int) SelectAccount.SelectedValue;
                var account = service.GetAccountById(id);
                var hours = account.AllottedTime/60;
                var minutes = account.AllottedTime%60;
                Hours.Text = hours.ToString();
                Minutes.Text = minutes.ToString();
                Tracking.IsChecked = account.Tracking;
            }
        }

    }
}
