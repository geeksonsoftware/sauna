using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Sauna.Server.Hubs
{
    public class SaunaHub : Hub
    {
        readonly SaunaMessenger _messenger;

        public SaunaHub(SaunaMessenger messenger)
        {
            this._messenger = messenger;
        }
        
        public Task TurnOn()
        {
            //notify the UI that the heater has been turned on
            return _messenger.TurnOn();
        }

        public Task TurnOff()
        {
            //notify the UI that the heater has been turned off
            return _messenger.TurnOff();
        }
        public Task EstimateETA(double targetTemperature)
        {
            return _messenger.UpdateTargetTemperature(targetTemperature);
        }

        public Task TemperatureUp(double targetTemperature)
        {
            return EstimateETA(targetTemperature);
        }

        public Task TemperatureDown(double targetTemperature)
        {
            return EstimateETA(targetTemperature);
        }
    }
}
