using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Globals
{
    public class Constants
    {
        public const string API_URL = @"https://myanumber-staging.herokuapp.com";

        public const string API_URL_PRIMARY_ADD = Constants.API_URL + @"/primary/add?";

        public const string API_URL_PRIMARY_VALIDATE = Constants.API_URL + @"/primary/validate?";

        public const string API_URL_DEBUG_GET = Constants.API_URL + @"/debug/get?";

        public const string TWINNING_API_CHANNEL = "TwinningChannel";
    }
}
