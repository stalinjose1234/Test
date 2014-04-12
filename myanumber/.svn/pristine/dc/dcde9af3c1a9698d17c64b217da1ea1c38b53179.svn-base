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

namespace myanumber
{
    public partial class EnterCode : PhoneApplicationPage
    {
        public EnterCode()
        {
            InitializeComponent();
            ValidationCode.MaxLength = (int)Enums.PhoneDigits.Six;
        }

        private bool isNextButtonEnabled;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            PhoneApplicationService.Current.State["ValidationCode"] = ValidationCode.Text.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);

                if (PhoneApplicationService.Current.State.ContainsKey("ValidationCode"))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(PhoneApplicationService.Current.State["ValidationCode"].ToString(), "[0-9]"))
                    {
                        ValidationCode.Text = PhoneApplicationService.Current.State["ValidationCode"].ToString();
                    }
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

        private void EnterPinCodeTextBox_KeyUP(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (ValidationCode.Text.ToString().Length == (int)Enums.PhoneDigits.Six)
                {
                    NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    isNextButtonEnabled = true;
                }
                else
                {
                    NextButton.Style = (Style)Application.Current.Resources["TwinningNextDisableButtonStyle"];
                    isNextButtonEnabled = false;
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }      

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isNextButtonEnabled)
                {
                    TwinningAPI apiWrapper = TwinningAPI.Instance;

                    // Passing in "12345" will return true, any other number will return false
                    apiWrapper.ValidateSecondaryDevice(ValidationCode.Text.ToString(), new Delegates.OnSuccessReturningStatusCallback(this.OnSecondaryValidationSuccess),
                                                    new Delegates.OnErrorCallback(this.OnError));
                    
                    //
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void OnSecondaryValidationSuccess(bool isValid)
        {
            if (isValid)
            {
                TwinningAPI apiWrapper = TwinningAPI.Instance;

                // Passing in blank will throw an "OnAttConsentError".  Passing in "twinerror" will throw an "OnCreateTwinError". 
                // Passing in anything else will be a success.
                apiWrapper.CreateTwin("1343141324123434", new Delegates.OnSuccessCallback(this.OnCreateTwinSuccess),
                                new Delegates.OnAttConsentErrorCallback(this.OnAttConsentError),
                                new Delegates.OnCreateTwinErrorCallback(this.OnCreateTwinError));
            }
            else
            {
                // Ask user to re-enter the authorization code from the secondary device OR
                // re-execute CheckIfSecondaryDeviceExists command to send out a new text
                // with a new authorization code for the user to use.               
            }
        }

        private void OnCreateTwinSuccess()
        {
            try
            {
                NavigationService.Navigate(new Uri("/OAuthAuthentication.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnAttConsentError(string message)
        {
            DispatchInvoke(() =>
            {
                MessageBox.Show(message);
            });
        }

        private void OnCreateTwinError(string message)
        {
            DispatchInvoke(() =>
            {
                MessageBox.Show(message);
            });
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
       
        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                //MessageBox.Show("Tabbed");
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidationCode.Focus();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void EnterPinCodeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
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
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}