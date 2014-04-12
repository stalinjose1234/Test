using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Notification;
using System.IO;

namespace TwinningAPIWrapper.Helpers
{
    internal class NotificationHelper
    {
        private OnPushUriUpdated _onUriUpdateCallback;
        private OnPushNotificationReceived _onNotificationReceivedCallback;
        private OnPushError _onErrorCallback;
        
        public delegate void OnPushUriUpdated(string uri);
        public delegate void OnPushNotificationReceived(string message);
        public delegate void OnPushError(string message);

        public NotificationHelper()
        {
            
        }

        public void OpenNotificationChannel(string channelName, OnPushUriUpdated onUriUpdatedCallback, 
                                    OnPushNotificationReceived onNotificationReceivedCallback, OnPushError onErrorCallback)
        {
            this.SetChannelCallbacks(onUriUpdatedCallback, onNotificationReceivedCallback, onErrorCallback);
            this.OpenChannel(channelName);
        }

        public void CloseNotificationChannel(string channelName)
        {
            HttpNotificationChannel pushChannel = HttpNotificationChannel.Find(channelName);

            if (pushChannel != null)
            {
                pushChannel.Close();
            }
        }

        private void SetChannelCallbacks(OnPushUriUpdated onUriUpdatedCallback, OnPushNotificationReceived onNotificationReceivedCallback,
                                    OnPushError onErrorCallback)
        {
            this._onUriUpdateCallback = onUriUpdatedCallback;
            this._onNotificationReceivedCallback = onNotificationReceivedCallback;
            this._onErrorCallback = onErrorCallback;
        }

        private void OpenChannel(string channelName)
        {
            HttpNotificationChannel pushChannel = HttpNotificationChannel.Find(channelName);

            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
                pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(this.PushChannel_HttpNotificationReceived);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(this.PushChannel_ErrorOccurred);

                pushChannel.Open();
            }
            else
            {
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
                pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(this.PushChannel_HttpNotificationReceived);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(this.PushChannel_ErrorOccurred);

                if (!(string.IsNullOrEmpty(pushChannel.ChannelUri.ToString())))
                {
                    if (this._onUriUpdateCallback != null)
                    {
                        this._onUriUpdateCallback(pushChannel.ChannelUri.ToString());
                    }
                }
            }
        }

        private void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            if (this._onUriUpdateCallback != null)
            {
                this._onUriUpdateCallback(e.ChannelUri.ToString());
            }
        }

        private void PushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            string message = string.Empty;

            using (StreamReader reader = new StreamReader(e.Notification.Body))
            {
                message = reader.ReadToEnd();
            }

            if (this._onNotificationReceivedCallback != null)
            {
                this._onNotificationReceivedCallback(message);
            }
        }

        private void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            if (this._onErrorCallback != null)
            {
                this._onErrorCallback(e.Message);
            }
        }
    }
}
