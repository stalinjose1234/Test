using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Models
{
    public class TwinningInformation
    {
        public string PrimaryDevicePhoneNumber { get; private set; }
        public string SecondaryDevicePhoneNumber { get; private set; }

        public bool TwinningStatus { get; private set; }
        public bool CallForwardingStatus { get; private set; }

        public TwinningInformation(Twin data)
        {
            if (data != null)
            {
                this.PrimaryDevicePhoneNumber = data.PrimaryDevicePhoneNumber;
                this.SecondaryDevicePhoneNumber = data.SecondaryDevicePhoneNumber;
                this.TwinningStatus = data.TwinningStatus;
                this.CallForwardingStatus = data.CallForwardingStatus;
            }
        }
    }
}
