using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinningAPIWrapper.Models;
using TwinningAPIWrapper.Globals;

namespace TwinningAPIWrapper.Interfaces
{
    interface IAPIWrapper
    {
        bool IsTwinningEstablished();

        void AddPrimaryDevice(string primaryDevicePhoneNumber, Delegates.OnSuccessCallback onSuccess, Delegates.OnErrorCallback onError);

        void CheckIfSecondaryDeviceExists(string secondaryDevicePhoneNumber, Delegates.OnSuccessReturningStatusCallback onSuccess, Delegates.OnErrorCallback onError);

        void ValidateSecondaryDevice(string secondaryAuthorizationCode, Delegates.OnSuccessReturningStatusCallback onSuccess, Delegates.OnErrorCallback onError);

        void CreateTwin(string attOAuthAuthorizationCode, Delegates.OnSuccessCallback onSuccess,
                        Delegates.OnAttConsentErrorCallback onAttConsentError, Delegates.OnCreateTwinErrorCallback onCreateTwinError);

        void UpdateTwinningStatus(bool desiredTwinningStatus, Delegates.OnSuccessReturningStatusCallback onSuccess, Delegates.OnErrorCallback onError);

        void DeleteTwin(Delegates.OnSuccessCallback onSuccess, Delegates.OnErrorCallback onError);

        TwinningInformation GetTwinningInformation();

        string GetPrimaryDevicePhoneNumber();
    }
}
