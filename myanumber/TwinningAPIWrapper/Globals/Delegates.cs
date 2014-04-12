using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Globals
{
    public class Delegates
    {
        public delegate void OnSuccessCallback();
        public delegate void OnSuccessReturningStatusCallback(bool status);

        public delegate void OnErrorCallback(string message);

        public delegate void OnAttConsentErrorCallback(string message);
        public delegate void OnCreateTwinErrorCallback(string message);
    }
}
