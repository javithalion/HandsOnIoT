using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.PredictionsModel
{
    public interface IDeviceEventsFeederService
    {
        Task RetrainMachineLearningWithYesterdaysEventsAsync(long maxNumberOfEvents = long.MaxValue);
    }
}