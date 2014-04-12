using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TwinningAPIWrapper;
using TwinningAPIWrapper.Globals;
using myanumber.Entities;
namespace myanumber
{
    public partial class EnterPin : PhoneApplicationPage
    {
        private static bool isNextButtonEnabled;
        TwinningData twinningData;
        public EnterPin()
        {
            InitializeComponent();
            twinningData = new TwinningData();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);

                if (PhoneApplicationService.Current.State.ContainsKey("FormattedSecondaryPhoneNumber"))
                {
                    twinningData.SecondaryDevicePhoneNumber = PhoneApplicationService.Current.State["FormattedSecondaryPhoneNumber"].ToString();
                    SecondaryPhoneNumber.DataContext = twinningData;
                }

                if (PhoneApplicationService.Current.State.ContainsKey("ValidationCode"))
                {
                    ValidationCode.Text = (string)PhoneApplicationService.Current.State["ValidationCode"];
                }

                if (ValidationCode.Text.ToString().Length == (int)Enums.PhoneDigits.Six)
                {
                    NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    isNextButtonEnabled = true;
                }
                else
                {
                    isNextButtonEnabled = false;
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void PinCodeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/EnterCode.xaml", UriKind.Relative));
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ValidateCode(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isNextButtonEnabled)
                {
                    TwinningAPI apiWrapper = TwinningAPI.Instance;

                    // Passing in "12345" will return true, any other number will return false
                    apiWrapper.ValidateSecondaryDevice(ValidationCode.Text.ToString(), new Delegates.OnSuccessReturningStatusCallback(this.OnSecondaryValidationSuccess),
                                                    new Delegates.OnErrorCallback(this.OnError));                                      
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private void OnSecondaryValidationSuccess(bool isValid)
        {
            if (isValid)
            {
                NavigationService.Navigate(new Uri("/OAuthAuthentication.xaml", UriKind.Relative));
            }
            else
            {
                // Ask user to re-enter the authorization code from the secondary device OR
                // re-execute CheckIfSecondaryDeviceExists command to send out a new text
                // with a new authorization code for the user to use.

                DispatchInvoke(() =>
                {
                    MessageBox.Show("The secondary authorization code is incorrect!");
                });
            }
        }

        private void OnError(string message)
        {
            DispatchInvoke(() =>
            {
                MessageBox.Show(message);
            });
        }

        public void DispatchInvoke(Action a)
        {
#if SILVERLIGHT
            if (Dispatcher == null)
                a();
            else
                Dispatcher.BeginInvoke(a);
#else
            if ((Dispatcher != null) && (!Dispatcher.HasThreadAccess))
            {
                Dispatcher.InvokeAsync(
                            Windows.UI.Core.CoreDispatcherPriority.Normal, 
                            (obj, invokedArgs) => { a(); }, 
                            this, 
                            null
                 );
                        }
            else
                a();
#endif
        }
    }
}