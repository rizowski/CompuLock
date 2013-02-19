using System;
using System.Windows;
using Service;

namespace CompuLockDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainService mainService;
        public MainWindow()
        {
            InitializeComponent();
            mainService = new MainService();
        }

        private void OnOpen(object sender, EventArgs e)
        {

        }

    }
}
