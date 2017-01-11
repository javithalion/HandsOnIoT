using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel
{
    public class PredictionsService : IPredictionsService
    {
        private int NumberOfHours = 24;

        public async Task<HourlySwitchedOnPredictionDto> HourlySwitchedOnAsync(DateTime date)
        {
            var result = new HourlySwitchedOnPredictionDto();

            result.Date = date;

            for (int currentHour = 0; currentHour < NumberOfHours; currentHour++)
            {
                var predictedNumberOfDevices = await GetSwitchedOnDevicesAsync(date, currentHour);
                result.HourlyForecast.Add(currentHour, predictedNumberOfDevices);
            }

            return result;
        }

        private async Task<int> GetSwitchedOnDevicesAsync(DateTime date, int currentHour)
        {
            return new Random().Next(0,200);
        }
    }
}
