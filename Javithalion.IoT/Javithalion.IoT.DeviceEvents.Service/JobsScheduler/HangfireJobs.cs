using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Serilog;
using Javithalion.IoT.DeviceEvents.Business.PredictionsModel;

namespace Javithalion.IoT.DeviceEvents.Service.JobsScheduler
{
    public class RetrainMachineLearningModelJob
    {
        private const string JobKey = "ML_DailyEventsRetrainModel";

        public static void Start()
        {
            RecurringJob.AddOrUpdate(JobKey,
                                     (IDeviceEventsFeederService feederService) =>
                                        feederService.RetrainMachineLearningWithYesterdaysEventsAsync(long.MaxValue).ConfigureAwait(false),
                                     Cron.Daily(1, 30),
                                     TimeZoneInfo.Utc);
        }

        public static void Stop()
        {
            RecurringJob.RemoveIfExists(JobKey);
        }
    }
}
