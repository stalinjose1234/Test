﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using TwinningAPIWrapper;
using TwinningAPIWrapper.Globals;
using myanumber.Enums;
using myanumber.Entities;

namespace myanumber
{
    public partial class EnterLinkedNumber : PhoneApplicationPage
    {
        TwinningData twinningData;
        public EnterLinkedNumber()
        {
            InitializeComponent();
            //EnterLinkedNumberTextBox.Focus();
            twinningData = new TwinningData();
        }

        string formattedPhoneNumber;
        private bool isNextButtonEnabled;
       
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedFrom(e);
                PhoneApplicationService.Current.State["SecondaryPhoneNumber"] = SecondaryPhoneNumber.Text.ToString();
                PhoneApplicationService.Current.State["FormattedSecondaryPhoneNumber"] = formattedPhoneNumber;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                if (PhoneApplicationService.Current.State.ContainsKey("SecondaryPhoneNumber"))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(PhoneApplicationService.Current.State["SecondaryPhoneNumber"].ToString(), "[0-9]"))
                    {
                        twinningData.SecondaryDevicePhoneNumber = PhoneApplicationService.Current.State["SecondaryPhoneNumber"].ToString();
                        SecondaryPhoneNumber.DataContext = twinningData;
                    }                   
                }
                if (SecondaryPhoneNumber.Text.ToString().StartsWith("0") || SecondaryPhoneNumber.Text.ToString().StartsWith("1"))
                {
                    if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Eleven)
                    {
                        NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    }
                }
                else
                {
                    if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Ten)
                    {
                        NextButton.Style = (Style)Application.Current.Resources["TwinningNextEnableButtonStyle"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      
        private void LinkedNumberTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (SecondaryPhoneNumber.Text.ToString().StartsWith("0") || SecondaryPhoneNumber.Text.ToString().StartsWith("1"))
                {
                    //SecondaryPhoneNumber.MaxLength = (int)Number.Eleven;

                    if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Eleven)
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
                else
                {
                    //SecondaryPhoneNumber.MaxLength = (int)Number.Ten;

                    if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Ten)
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }        

        private void LinkedNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Ten)
                {
                    string userInput = SecondaryPhoneNumber.Text;

                    Regex regexPhoneNumber = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");

                    if (regexPhoneNumber.IsMatch(userInput))
                    {
                        formattedPhoneNumber = regexPhoneNumber.Replace(userInput, "($1) $2-$3");
                        //MessageBox.Show(formattedPhoneNumber);
                    }
                }
                else if (SecondaryPhoneNumber.Text.ToString().Length == (int)Enums.PhoneDigits.Eleven)
                {
                    string userInput = SecondaryPhoneNumber.Text;

                    Regex regexPhoneNumber = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
                    if (regexPhoneNumber.IsMatch(userInput))
                    {
                        formattedPhoneNumber = regexPhoneNumber.Replace(userInput, "$1 ($2) $3-$4");
                        //MessageBox.Show(formattedPhoneNumber);
                    }
                }
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
                SecondaryPhoneNumber.Focus();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void EnterLinkedNumberTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isNextButtonEnabled)
                {
                    //NavigationService.Navigate(new Uri("/EnterPin.xaml", UriKind.Relative));

                    TwinningAPI apiWrapper = TwinningAPI.Instance;
                    // Check if the secondary device number is already registered
                    // Pass in "5552321212" to get an true response, any other number will return false 
                    apiWrapper.CheckIfSecondaryDeviceExists(SecondaryPhoneNumber.Text.ToString(), new Delegates.OnSuccessReturningStatusCallback(this.OnSecondaryExistsSuccess),
                                               new Delegates.OnErrorCallback(this.OnError));
                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }


        private void OnSecondaryExistsSuccess(bool doesExist)
        {
            try
            {
                if (doesExist)
                {
                    NavigationService.Navigate(new Uri("/EnterPin.xaml", UriKind.Relative));
                }
                else
                {                   
                    // try again.                   
                    MessageBox.Show("The secondary device is not registered with the Twinning Server API!");                   
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
       

        private void OnError(string message)
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
    }
}