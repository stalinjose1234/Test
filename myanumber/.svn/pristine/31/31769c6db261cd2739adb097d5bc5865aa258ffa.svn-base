using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Models.DataAccess
{
    internal class LocalStorageAccess
    {
        private Twin _localTwinData;

        public Twin LocalTwinData
        {
            get
            {
                if (this._localTwinData == null)
                {
                    this._localTwinData = this.LoadTwinData();
                }

                return this._localTwinData;
            }
            
        }

        public Twin GetLocalTwinDataFromStorage()
        {
            return this.LoadTwinData();
        }

        public bool SaveLocalTwinData()
        {
            //save

            //now that it's been saved, update the _localTwinData to latest
            this._localTwinData = this.LoadTwinData();

            return true;
        }

        private Twin LoadTwinData()
        {
            return new Twin();
        }
    }
}
