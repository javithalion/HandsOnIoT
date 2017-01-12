using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel.DTOs
{
    public class HourlySwitchedOnPredictionDto
    {
        public HourlySwitchedOnPredictionDto()
        {
            HourlyForecast = new Dictionary<int, int>();
        }
        public DateTime Date { get; set; }

        public IDictionary<int, int> HourlyForecast { get; set; }
    }
}
