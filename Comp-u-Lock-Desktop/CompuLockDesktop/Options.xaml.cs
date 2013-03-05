using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Database.Models;
using REST;
using Service;

namespace CompuLockDesktop
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options
    {
        private MainService service;
        public Options(MainService service)
        {
            this.service = service;
            InitializeComponent();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            var user = service.GetDbUser();
            if (user != null)
                AuthToken.Text = user.AuthToken;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (AuthToken.Text.Length <= 0) return;
            try
            {
                var dbuser = service.GetDbUser();
                if(dbuser == null)
                {
                    service.SaveUserToDb(AuthToken.Text);
                }
                else
                {
                    dbuser.AuthToken = AuthToken.Text;
                    service.UpdateDbUser(dbuser);
                }

            }
            catch (ServerOfflineException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            AuthToken.Text = "";
        }
    }
}
