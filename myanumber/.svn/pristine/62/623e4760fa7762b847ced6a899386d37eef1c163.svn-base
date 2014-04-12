using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinningAPIWrapper.Globals;
using TwinningAPIWrapper.Models.WebResponseModels;

namespace TwinningAPIWrapper.Helpers
{
    internal class WebAPIHelper
    {
        public static void HttpAddPrimaryDevice(string primaryDevicePhoneNumber, string primaryNotificationKey,
            HttpHelper.OnHttpSuccessCallback onHttpSuccess, HttpHelper.OnHttpErrorCallback onHttpError)
        {
            var postParameters = new Dictionary<string, string>();

            postParameters.Add("phone_number", primaryDevicePhoneNumber);
            postParameters.Add("notification_key", primaryNotificationKey);
            postParameters.Add("notification_type", "wns");

            HttpHelper.HttpPost(Constants.API_URL_PRIMARY_ADD, postParameters, 
                (string addResponse) => 
                {
                    WebAPIHelper.HttpDebugGetPendingByPhoneNumber(primaryDevicePhoneNumber, 
                        (string debugResponse) => 
                        {
                            DebugDeviceRecord device = WebResponseHelper.GetLatestDeviceFromDebugGetResponse(debugResponse);
                            WebAPIHelper.HttpValidatePrimaryDevice(device.device_access_key, device.notification_validation_code, 
                                onHttpSuccess, onHttpError);
                        }, onHttpError);
                }, 
                onHttpError);
        }

        public static void HttpValidatePrimaryDevice(string primaryDeviceAccessKey, string primaryNotificationValidationCode,
            HttpHelper.OnHttpSuccessCallback onHttpSuccess, HttpHelper.OnHttpErrorCallback onHttpError)
        {
            var postParameters = new Dictionary<string, string>();

            postParameters.Add("device_access_key", primaryDeviceAccessKey);
            postParameters.Add("notification_validation_code", primaryNotificationValidationCode);

            HttpHelper.HttpPost(Constants.API_URL_PRIMARY_VALIDATE, postParameters, onHttpSuccess, onHttpError);
        }

        public static void HttpDebugGetPendingByPhoneNumber(string phoneNumber, HttpHelper.OnHttpSuccessCallback onHttpSuccess,
            HttpHelper.OnHttpErrorCallback onHttpError)
        {
            string debugUrl = Constants.API_URL_DEBUG_GET + "key=sonos&table=pending&phone_number=" + phoneNumber;

            HttpHelper.HttpGet(debugUrl, onHttpSuccess, onHttpError);
        }        
    }
}
