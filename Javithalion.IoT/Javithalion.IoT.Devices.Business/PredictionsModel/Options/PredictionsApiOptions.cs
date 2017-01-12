using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.PredictionsModel.Options
{
    public class PredictionsApiOptions
    {
        public string ApiUrl { get; private set; }
        public string ApiKey { get; private set; }

        public PredictionsApiOptions()
        {
            ApiUrl = string.Empty;
            ApiKey = string.Empty;
        }

        public PredictionsApiOptions WithApiUrl(string url)
        {
            url = url ?? string.Empty;
            ApiUrl = url;

            return this;
        }

        public PredictionsApiOptions WithApiKey(string key)
        {
            key = key ?? string.Empty;
            ApiKey = key;

            return this;
        }
    }
}
