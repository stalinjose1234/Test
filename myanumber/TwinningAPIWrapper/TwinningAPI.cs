﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinningAPIWrapper.Interfaces;
using TwinningAPIWrapper.Models;
using TwinningAPIWrapper.Models.DataAccess;
using TwinningAPIWrapper.Globals;
using TwinningAPIWrapper.Helpers;

namespace TwinningAPIWrapper
{
    public class TwinningAPI : IAPIWrapper
    {
        private static TwinningAPI instance;

        private NotificationHelper _notify;

        private LocalStorageAccess _dataAccess;

        private TwinningAPI()
        {
            this._notify = new NotificationHelper();
            this._dataAccess = new LocalStorageAccess();
        }

        public static TwinningAPI Instance
        {
            get
            {
                if (TwinningAPI.instance == null)
                {
                    instance = new TwinningAPI();
                }

                return instance;
            }
        }

        public bool IsTwinningEstablished()
        {
            Twin data = this._dataAccess.LocalTwinData;

            return (!string.IsNullOrEmpty(data.TwinningId) && !(string.IsNullOrEmpty(data.PrimaryDeviceAccessKey)));
        }

        public void AddPrimaryDevice(string primaryDevicePhoneNumber, Delegates.OnSuccessCallback onSuccess, Delegates.OnErrorCallback onError)
        {
            this._dataAccess.LocalTwinData.PrimaryDevicePhoneNumber = primaryDevicePhoneNumber;

            this._notify.OpenNotificationChannel(Constants.TWINNING_API_CHANNEL,
                // Uri Updated
                (string uri) =>
                {
                    WebAPIHelper.HttpAddPrimaryDevice(primaryDevicePhoneNumber, uri,
                        (string response) =>
                        {
                            string deviceAccessKey = WebResponseHelper.GetDeviceAccessKeyFromResponse(response);
                            if (!string.IsNullOrWhiteSpace(deviceAccessKey))
                            {
                                this._dataAccess.LocalTwinData.PrimaryDeviceAccessKey = deviceAccessKey;
                                onSuccess();
                            }
                            else
                            {
                                onError("An error occurred while generating a primary device access key");
                            }

                        },
                        (string message) => { onError(message); });
                },
                // Notification Received
                (string message) => { onError("Message: " + message); },
                // Error
                (string message) => { onError(message); });
        }

        public void CheckIfSecondaryDeviceExists(string secondaryDevicePhoneNumber, Delegates.OnSuccessReturningStatusCallback onSuccess,
                                                Delegates.OnErrorCallback onError)
        {
            if (string.IsNullOrWhiteSpace(secondaryDevicePhoneNumber))
            {
                onError("The secondary phone number must not be null or empty.");
            }
            else if (secondaryDevicePhoneNumber == "5552321212")
            {
                onSuccess(true);
            }
            else
            {
                onSuccess(false);
            }
        }

        public void ValidateSecondaryDevice(string secondaryAuthorizationCode, Delegates.OnSuccessReturningStatusCallback onSuccess,
                                        Delegates.OnErrorCallback onError)
        {
            if (string.IsNullOrWhiteSpace(secondaryAuthorizationCode))
            {
                onError("The secondary authorization code must not be null or empty.");
            }
            else if (secondaryAuthorizationCode == "123456")
            {
                onSuccess(true);
            }
            else
            {
                onSuccess(false);
            }
        }

        public void CreateTwin(string attOAuthAuthorizationCode, Delegates.OnSuccessCallback onSuccess,
                        Delegates.OnAttConsentErrorCallback onAttConsentError, Delegates.OnCreateTwinErrorCallback onCreateTwinError)
        {
            if (string.IsNullOrWhiteSpace(attOAuthAuthorizationCode))
            {
                onAttConsentError("The att oauth authorization code must not be null or empty.");
            }
            else if (attOAuthAuthorizationCode == "twinerror")
            {
                onCreateTwinError("A twin could not be made");
            }
            else
            {
                onSuccess();
            }
        }

        public void UpdateTwinningStatus(bool desiredTwinningStatus, Delegates.OnSuccessReturningStatusCallback onSuccess, Delegates.OnErrorCallback onError)
        {
            onSuccess(desiredTwinningStatus);
        }

        public void DeleteTwin(Delegates.OnSuccessCallback onSuccess, Delegates.OnErrorCallback onError)
        {
            onSuccess();
        }

        public TwinningInformation GetTwinningInformation()
        {
            if (this.IsTwinningEstablished())
            {
                return new TwinningInformation(this._dataAccess.GetLocalTwinDataFromStorage());
            }

            return null;
        }

        public string GetPrimaryDevicePhoneNumber()
        {
            return "3031214466";
        }

        public bool GetPhoneSimNumberOrReturnNull()
        {
            return false;
        }

    }
}
