﻿using System;
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
using myanumber.Resources;
using System.IO.IsolatedStorage;
using myanumber.Entities;

namespace myanumber
{
    public partial class InitialTwinning : PhoneApplicationPage
    {
        bool isPrimaryDeviceRegistered;
        TwinningData twinningData;
        public InitialTwinning()
        {
            InitializeComponent();
            twinningData = new TwinningData ();
            try
            {
                TwinningAPI apiWrapper = TwinningAPI.Instance;
                LinkedNumberTextBox.Text = AppResources.SecondaryWaterMarkText;
                //PrimaryPhoneNumber.Text = apiWrapper.GetPrimaryDevicePhoneNumber();
                 var storage = IsolatedStorageSettings.ApplicationSettings;
                if (storage.Contains("PrimaryPhoneNumber"))
                {
                    twinningData.PrimaryDevicePhoneNumber = storage["PrimaryPhoneNumber"].ToString();
                    PrimaryPhoneNumber.DataContext = twinningData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                if (PhoneApplicationService.Current.State.ContainsKey("SecondaryPhoneNumber"))
                {
                    if (PhoneApplicationService.Current.State["SecondaryPhoneNumber"].ToString().Length != 0)
                    {
                       twinningData.SecondaryDevicePhoneNumber = PhoneApplicationService.Current.State["SecondaryPhoneNumber"].ToString();
                        LinkedNumberTextBox.DataContext = twinningData;
                    }
                }
                if (LinkedNumberTextBox.Text.ToString().StartsWith("0") || LinkedNumberTextBox.Text.ToString().StartsWith("1"))
                {
                    if (LinkedNumberTextBox.Text.ToString().Length == (int)Enums.PhoneDigits.Eleven)
                    {
                        NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    }
                }
                else
                {
                    if (LinkedNumberTextBox.Text.ToString().Length == (int)Enums.PhoneDigits.Ten)
                    {
                        NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
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

        private void TwinnningInitialPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TwinningAPI apiWrapper = TwinningAPI.Instance;              

                ////First check if the device is twinned
                //if (!(apiWrapper.IsTwinningEstablished()))
                //{
                //    //It is not twinned.
                //    //Register the device with the Server API
                //    apiWrapper.AddPrimaryDevice(new Delegates.OnSuccessCallback(this.OnPrimaryAddSuccess),
                //                            new Delegates.OnErrorCallback(this.OnError));
                   
                //}
                //else
                //{
                //    //It is twinned.                    
                //}
                var storage = IsolatedStorageSettings.ApplicationSettings;
                if (storage["PrimaryPhoneNumber"].ToString().Length > 0) 
                {
                    string primaryPhoneNumber = storage["PrimaryPhoneNumber"].ToString();
                   apiWrapper.AddPrimaryDevice(primaryPhoneNumber,new Delegates.OnSuccessCallback(this.OnPrimaryAddSuccess),
                                        new Delegates.OnErrorCallback(this.OnError));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
       
        private void OnPrimaryAddSuccess()
        {
            try
            {
                DispatchInvoke(() =>
                    {
                        //MessageBox.Show(TempDataResource.NumberRegistered);
                        isPrimaryDeviceRegistered = true;
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnSecondaryExistsSuccess(bool doesExist)
        {
            try
            {
                if (doesExist)
                {
                    TwinningAPI apiWrapper = TwinningAPI.Instance;

                    // Passing in "12345" will return true, any other number will return false
                    apiWrapper.ValidateSecondaryDevice("123456", new Delegates.OnSuccessReturningStatusCallback(this.OnSecondaryValidationSuccess),
                                                    new Delegates.OnErrorCallback(this.OnError));
                }
                else
                {
                    // Prompt user to turn on secondary device so that it may register with the Twinning Server API and then
                    // try again.

                    DispatchInvoke(() =>
                    {
                        MessageBox.Show("The secondary device is not registered with the Twinning Server API!");
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnSecondaryValidationSuccess(bool isValid)
        {
            try
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

                    DispatchInvoke(() =>
                    {
                        MessageBox.Show("The secondary authorization code is incorrect!");
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnCreateTwinSuccess()
        {
            try
            {
                // Restart the process.  This time when "isTwinningEstablished" is called, it should return true (however this does not work yet)
                DispatchInvoke(() =>
                {
                    MessageBox.Show("Twinning Created...but not really!");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnAttConsentError(string message)
        {
            try
            {
                DispatchInvoke(() =>
                {
                    MessageBox.Show(message);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnCreateTwinError(string message)
        {
            try
            {
                DispatchInvoke(() =>
                {
                    MessageBox.Show(message);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void OnError(string message)
        {
            try
            {
                DispatchInvoke(() =>
                {
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show(message," ", button);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void LinkedNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                LinkedNumberTextBox.Text = "";
                //if (isPrimaryDeviceRegistered)
                //{
                    NavigationService.Navigate(new Uri("/EnterLinkedNumber.xaml", UriKind.Relative));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void LinkedNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                LinkedNumberTextBox.Text = AppResources.SecondaryWaterMarkText;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TwinningAPI apiWrapper = TwinningAPI.Instance;

                // Check if the secondary device number is already registered

                // Pass in "5552321212" to get an true response, any other number will return false 
                apiWrapper.CheckIfSecondaryDeviceExists("5552321212", new Delegates.OnSuccessReturningStatusCallback(this.OnSecondaryExistsSuccess),
                                                    new Delegates.OnErrorCallback(this.OnError));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
       
    }
}