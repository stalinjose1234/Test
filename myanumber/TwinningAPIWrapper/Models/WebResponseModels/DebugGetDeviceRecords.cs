using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Models.WebResponseModels
{
    internal class DebugDeviceRecord
    {
        public string device_access_key { get; set; }
        public string device_access_key_dec { get; set; }
        public string phone_number { get; set; }
        public string notification_key { get; set; }
        public string notification_validation_code { get; set; }
        public string notification_type { get; set; }
        public DateTime created { get; set; }
    }

    internal class DebugGetDeviceRecords
    {
        public DebugDeviceRecord[] records;
    }
}
