using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TwinningAPIWrapper.Models.WebResponseModels;

namespace TwinningAPIWrapper.Helpers
{
    internal class WebResponseHelper
    {
        public static DebugDeviceRecord GetLatestDeviceFromDebugGetResponse(string jsonResponse)
        {
            var devices = JsonConvert.DeserializeObject<DebugGetDeviceRecords>("{ records: " + jsonResponse + "}");
            var orderedRecords = devices.records.OrderByDescending(r => r.created);

            return orderedRecords.First<DebugDeviceRecord>();
        }

        public static string GetDeviceAccessKeyFromResponse(string jsonResponse)
        {
            var deviceKey = JsonConvert.DeserializeObject<DeviceAccessKey>(jsonResponse);
            return deviceKey.device_access_key;
        }
    }
}
