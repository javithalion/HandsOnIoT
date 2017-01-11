using System;
using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel
{
    public interface IPredictionsService
    {
        Task<HourlySwitchedOnPredictionDto> HourlySwitchedOnAsync(DateTime date);
    }
}