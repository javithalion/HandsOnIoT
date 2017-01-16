using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.DataAccess.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.PredictionsModel
{
    public class DeviceEventsFeederService : IDeviceEventsFeederService
    {
        private readonly ILogger _logger;
        private readonly IDeviceEventDao _deviceEventsDao;

        public DeviceEventsFeederService(ILogger<DeviceEventsFeederService> logger, IDeviceEventDao deviceEventsDao)
        {
            _logger = logger;
            _deviceEventsDao = deviceEventsDao;
        }

        public async Task RetrainMachineLearningWithYesterdaysEventsAsync(long maxNumberOfEvents = long.MaxValue)
        {
            try
            {
                _logger.LogDebug("Starting daily ML sincronization and re-train");

                var todaysEvents = await _deviceEventsDao.AllDeviceEvents()
                                                         .FromYesterday()
                                                         .ToListAsync();
              
                //TODO :: Retrain model with yesterday events

                _logger.LogInformation("Daily ML sincronization and re-train properly finished");              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Daily ML sincronization and re-train failed. {ex.ToString()}");
                throw;
            }
        }
    }








    public class AzureBlobDataReference
    {
        // Storage connection string used for regular blobs. It has the following format:
        // DefaultEndpointsProtocol=https;AccountName=ACCOUNT_NAME;AccountKey=ACCOUNT_KEY
        // It's not used for shared access signature blobs.
        public string ConnectionString { get; set; }

        // Relative uri for the blob, used for regular blobs as well as shared access
        // signature blobs.
        public string RelativeLocation { get; set; }

        // Base url, only used for shared access signature blobs.
        public string BaseLocation { get; set; }

        // Shared access signature, only used for shared access signature blobs.
        public string SasBlobToken { get; set; }
    }

    public enum BatchScoreStatusCode
    {
        NotStarted,
        Running,
        Failed,
        Cancelled,
        Finished
    }

    public class BatchScoreStatus
    {
        // Status code for the batch scoring job
        public BatchScoreStatusCode StatusCode { get; set; }

        // Locations for the potential multiple batch scoring outputs
        public IDictionary<string, AzureBlobDataReference> Results { get; set; }

        // Error details, if any
        public string Details { get; set; }
    }

    public class BatchExecutionRequest
    {

        public IDictionary<string, AzureBlobDataReference> Inputs { get; set; }

        public IDictionary<string, string> GlobalParameters { get; set; }

        // Locations for the potential multiple batch scoring outputs
        public IDictionary<string, AzureBlobDataReference> Outputs { get; set; }
    }

















}
