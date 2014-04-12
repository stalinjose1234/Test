using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myanumber.Entities
{
    public class TwinningData : INotifyPropertyChanged
    {
        public string primaryDevicePhoneNumber;

        public string secondaryDevicePhoneNumber; 
        public string PrimaryDevicePhoneNumber
        {
            get
            {
                return primaryDevicePhoneNumber;
            }
            set
            {
                primaryDevicePhoneNumber = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PrimaryDevicePhoneNumber"));
                }
            }            
        }

        public string SecondaryDevicePhoneNumber
        {
            get
            {
                return secondaryDevicePhoneNumber;
            }
            set
            {
                secondaryDevicePhoneNumber = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SecondaryDevicePhoneNumber"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}
