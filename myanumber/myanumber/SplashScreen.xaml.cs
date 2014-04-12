using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace myanumber
{
    public partial class SplashScreen : PhoneApplicationPage
    {
        public SplashScreen()
        {
            InitializeComponent();

            this.SplashScreenLayout.Height =
             Application.Current.Host.Content.ActualHeight;

            this.SplashScreenLayout.Width =
            Application.Current.Host.Content.ActualWidth;

        }
    }
}