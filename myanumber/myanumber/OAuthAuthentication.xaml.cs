﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using TwinningAPIWrapper;
using TwinningAPIWrapper.Globals;

namespace myanumber
{
    public partial class OAuthAuthentication : PhoneApplicationPage
    {
        public OAuthAuthentication()
        {
            InitializeComponent();

            OAuthWebBrowser.IsScriptEnabled = true;
            OAuthWebBrowser.Navigate(new Uri("https://api.att.com/oauth/authorize?client_id=accw0hk0nyny1auwnevpa7h3rsnc6jyw&scope=IMMN,MIM"));

            OAuthWebBrowser.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(AuthBrowser_LoadCompleted);
            OAuthWebBrowser.ScriptNotify += new EventHandler<NotifyEventArgs>(AuthBrowser_ReceiveScriptNotify);
        }

        public string consentAddresssForSms = "";
        public string consentWordForSms = "";

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

        public void AuthBrowser_ReceiveScriptNotify(Object sender, NotifyEventArgs notifyArgs)
        {
            string val = notifyArgs.Value;
            this.consentWordForSms = val.Split('&')[0];
            this.consentAddresssForSms = val.Split('&')[1];

            ButtonSendSms.Visibility = Visibility.Visible;
        }


        public void AuthBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {

            // Hide the AT&T id tab if webbrowser is showing consent page
            if (OAuthWebBrowser.Source.AbsoluteUri.Contains("consent"))
            {

                OAuthWebBrowser.InvokeScript("eval",
                    new string[] { "( function(){" +
                        "var _twinning_lastElements = document.getElementsByClassName('last');" +
                        "for(var _twinning_i=0; _twinning_i < _twinning_lastElements.length; _twinning_i++)" +
                        "{_twinning_lastElements[_twinning_i].style.visibility = 'hidden'; } " +
                        
                        "var _twinning_consentWords = document.getElementsByClassName('consent_word');" +
                        "var _twinning_consentAddresses = document.getElementsByClassName('consent_address');" +
                        "var _twinning_consentWord = '' ; " +
                        "var _twinning_consentAddress = '' ; " +
                        "if( _twinning_consentWords.length > 0) {_twinning_consentWord = _twinning_consentWords[0].textContent;}" +
                        "if( _twinning_consentAddresses.length > 0) {_twinning_consentAddress = _twinning_consentAddresses[0].textContent;}" +
                        "window.external.Notify(_twinning_consentWord + '&' + _twinning_consentAddress);" +
                        "})();" 
                        
                        //" var _twinning_smsButton = document.getElementById('Needbuttonidhere');" + 
                        // Get Button id, not showing up in desktop version of AT&T page
                        //" if(_twinning_snsButton){" +
                        //"{_twinning_snsButton.addEventListener('click', function(){window.external.notify(_twinning_consentWord + '&' + _twinning_consentAddress);}, false);}" 
                        
                    });
            }
            else if (OAuthWebBrowser.Source.AbsoluteUri.Contains("myanumber"))
            {
                string code = "";
                string queryString = OAuthWebBrowser.Source.AbsoluteUri.Split('?')[1];
                string[] queryStringParams = queryString.Split('&');
                foreach (string queryStringParamEach in queryStringParams)
                {
                    if (queryStringParamEach.Split('=')[0] == "code")
                    {
                        code = queryStringParamEach.Split('=')[1];
                    }
                }

                if (code != "")
                {
                    // Call MyANumber API here with code
                    TwinningAPI apiWrapper = TwinningAPI.Instance;
                    apiWrapper.CreateTwin("twinerror", new Delegates.OnSuccessCallback(this.OnSuccess),
                        new Delegates.OnAttConsentErrorCallback(this.OnAttConsentError),
                        new Delegates.OnCreateTwinErrorCallback(this.OnCreateTwinError));
                        
                }
                else
                {
                    // Show error
                }
            }

        }

        private void Button_SendSMS(object sender, RoutedEventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = this.consentAddresssForSms;
            smsComposeTask.Body = this.consentWordForSms;
            smsComposeTask.Show();
        }

        /*private void Button_Revoke(object sender, RoutedEventArgs e)
        {
            OAuthWebBrowser.Navigate(new Uri("https://auth-api.att.com/permissions/manage"));
        } */// end on load
       
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Twinning.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnSuccess()
        {
            try
            {
                // Restart the process.  This time when "isTwinningEstablished" is called, it should return true (however this does not work yet)
                DispatchInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Twinning.xaml", UriKind.Relative));
                });
            }
            catch (Exception ex)
            {
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
                throw;
            }
        }
    }
}