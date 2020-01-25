using Iot.Units;
using Microsoft.AspNetCore.SignalR;
using Sauna.RPI.Controlling;
using System;
using System.Threading.Tasks;

namespace Sauna.RPI.Web
{
    class Messenger
    {
        readonly IHubContext<SaunaHub> _hub;

        public Messenger(IHubContext<SaunaHub> hub)
        {
            _hub = hub;

            SaunaController.GetInstance().TemperatureRead += OnTemperatureUpdated;
        }

        void OnTemperatureUpdated(DateTime time, Temperature internalTemperature, Temperature externalTemperature)
        {
            _ = BroadcastNewTemperature(time, internalTemperature, externalTemperature);
        }

        internal async Task BroadcastNewTemperature(DateTime time, Temperature internalTemperature, Temperature externalTemperature)
        {
            await _hub.Clients.All.SendAsync("NewTemperature", DateTime.Now.Second);
        }
    }
}
