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
        }

        private void OnOpen(object sender, EventArgs e)
        {
            //Get User
            //Get computers
            //get accounts
            //
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Options();
            dialog.ShowDialog();
            if (dialog.Activate() == true)
            {
                
            }
        }

    }
}
