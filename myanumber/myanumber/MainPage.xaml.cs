using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using myanumber.Resources;
using myanumber.ViewModels;


namespace myanumber
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            MessageBox.Show("Started");

            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("InitializeComponent" + ex.Message);
                throw;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/InitialTwinning.xaml", UriKind.Relative));
            }
            catch (Exception ex) 
            {

                MessageBox.Show("AcceptButton_Click"+ex.Message);
                throw;
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("You cannot proceed without accepting terms.", "", button);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DeclineButton_Click" + ex.Message);
                throw;
            }
        }
    }
}