using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Javithalion.IoT.Devices.Business.PredictionsModel.Options;

namespace Javithalion.IoT.Devices.Business.ReadModel
{
    public class PredictionsService : IPredictionsService
    {
        private const string PredictionKeyPrefix = "HourlySwitchedOnFor_";
        private const int NumberOfHours = 24;
        private const int PredictionExpirationTimeInHours = 12;

        private readonly IMemoryCache _predictionCache;
        private readonly PredictionsApiOptions _options;

        public PredictionsService(IMemoryCache predictionCache, IOptions<PredictionsApiOptions> options)
        {
            _predictionCache = predictionCache;
            _options = options.Value;
        }

        public async Task<HourlySwitchedOnPredictionDto> HourlySwitchedOnAsync(DateTime date)
        {
            HourlySwitchedOnPredictionDto result;
            var expectedCacheKey = $"{PredictionKeyPrefix}{date.Date.ToString()}";

            if (!_predictionCache.TryGetValue(expectedCacheKey, out result))
            {
                result = await GetHourlyPredictionAsync(date);
                _predictionCache.Set(expectedCacheKey, result, TimeSpan.FromHours(PredictionExpirationTimeInHours));
            }

            return result;
        }

        private async Task<HourlySwitchedOnPredictionDto> GetHourlyPredictionAsync(DateTime date)
        {
            var result = new HourlySwitchedOnPredictionDto();

            var predictedNumberOfDevicesPerHour = await GetSwitchedOnDevicesPredictionAsync(date);
            foreach (var prediction in predictedNumberOfDevicesPerHour)
            {
                result.HourlyForecast.Add(prediction.Key, prediction.Value);
            }

            result.Date = date;

            return result;
        }

        private async Task<IDictionary<int, int>> GetSwitchedOnDevicesPredictionAsync(DateTime date)
        {
            using (var client = new HttpClient())
            {
                HttpContent requestContent = GetRequestContentForSwitchedOnDevicesPrediction(date);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
                var uri = new Uri(_options.ApiUrl);

                HttpResponseMessage response = await client.PostAsync(uri, requestContent);

                return await GetPredictionFromResponse(response);
            }
        }

        private static async Task<IDictionary<int, int>> GetPredictionFromResponse(HttpResponseMessage httpResponse)
        {
            var result = new Dictionary<int, int>();

            for (int i = 0; i < NumberOfHours; i++) //Initialize            
                result.Add(i, 0);

            try
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    string response = await httpResponse.Content.ReadAsStringAsync();
                    var parsedResponse = JObject.Parse(response);

                    foreach (var hourPrediction in parsedResponse["Results"]["output1"].Children())
                    {
                        var hour = int.Parse(hourPrediction["CAST(strftime('%H',t1.Date) as decimal)"].Value<string>());
                        var prediction = (int)Math.Round(double.Parse(hourPrediction["Scored Label Mean"].Value<string>()), 0);

                        if (result.ContainsKey(hour))
                            result[hour] = prediction;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO :: Log
            }

            return result;
        }

        private HttpContent GetRequestContentForSwitchedOnDevicesPrediction(DateTime date)
        {
            var inputRequest = new List<Dictionary<string, string>>();
            for (int i = 0; i < NumberOfHours; i++)
            {
                inputRequest.Add(
                    new Dictionary<string, string>()
                                {
                                 {"cast (strftime('%w', t1.Date) as decimal)", $"{(int)date.DayOfWeek}"},
                                 {"CAST(strftime('%H',t1.Date) as decimal)", $"{i}"},
                                 {"count(*)", "0"},
                                });
            }


            var scoreRequest = new
            {
                Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                {
                   {"input1", inputRequest}
                 },
                GlobalParameters = new Dictionary<string, string>()
            };


            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(scoreRequest);
            return new StringContent(jsonContent);
        }
    }
}
