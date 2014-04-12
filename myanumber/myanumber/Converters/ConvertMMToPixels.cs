using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using myanumber.Entities;
using Microsoft.Phone.Info;

namespace myanumber.Converters
{
    public class ConvertMMToPixels : IValueConverter
    {
        private static int oneCM = 10;
        private static double oneInch = 2.54;
        private static double PPIFor925 = 334;
        private static double scaleFor925 = 1.6;
        private static double PPIFor1520 = 368;
        private static double scaleFor1520 = 2.25;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return 65.50;// ConvertIntoPixels(value);

          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion


        private object ConvertIntoPixels(object value)
        {
                                 

             DeviceDetails deviceDetails;
             deviceDetails = GetPPIScaleForCurrentDevice();

             double valueInMM = (double)value;

            double valueInInches = valueInMM / oneCM / oneInch;
            return (object) (valueInInches * deviceDetails.PPI / deviceDetails.Scale);
        }

        private DeviceDetails GetPPIScaleForCurrentDevice()
        {
            DeviceDetails deviceDetails = new DeviceDetails();
            //string deviceName = "RM-892_im-india_216";         
            string deviceName = DeviceStatus.DeviceName;
            string deviceValue = deviceName.Split('_')[0];

            switch (deviceValue)
            {
                //Device Names fror Nokia Lumia 925
                case "RM-892":
                case "RM-893":
                case "RM-910":
                    deviceDetails.PPI = PPIFor925;
                    deviceDetails.Scale = scaleFor925;
                    break;
                //Device Names for Nokia Lumia 1520
                case "RM-937":
                case "RM-940":
                case "RM-939":
                    deviceDetails.PPI = PPIFor1520;
                    deviceDetails.Scale = scaleFor1520;
                    break;
                default:
                    deviceDetails.PPI = 0;
                    deviceDetails.Scale = 0;
                    break;
            }

            return deviceDetails;

        }
    }
}

