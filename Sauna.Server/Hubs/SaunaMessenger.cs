using Iot.Units;
using Microsoft.AspNetCore.SignalR;
using Sauna.Core;
using Sauna.RPI.Controlling;
using System;
using Sauna.Server.Extensions;
using System.Threading.Tasks;

namespace Sauna.Server.Hubs
{
    public class SaunaMessenger
    {
        readonly IHubContext<SaunaHub> _hubContext;

        SaunaController _sauna;

        public SaunaMessenger(IHubContext<SaunaHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void WireSauna(SaunaController sauna)
        {
            _sauna = sauna;

            _sauna.TemperatureRead += _sauna_TemperatureRead;
        }

        void _sauna_TemperatureRead(DateTime timestamp, Iot.Units.Temperature internalTemperature, Iot.Units.Temperature externalTemperature)
        {
            var reading = new TemperatureReading() { Time = timestamp, Internal = internalTemperature.ToSaunaTemperature(), External = externalTemperature.ToSaunaTemperature() };

            _hubContext.Clients.All.SendAsync("NewTemperatureReading", reading);
        }

        internal async Task TurnOn()
        {
            await _sauna.TurnOn();

            await _hubContext.Clients.All.SendAsync("UpdateSaunaStatus", true);
        }

        internal async Task TurnOff()
        {
            await _sauna.TurnOff();

            await _hubContext.Clients.All.SendAsync("UpdateSaunaStatus", false);
        }

        internal async Task UpdateTargetTemperature(double targetTemperature)
        {
            await _sauna.UpdateTargetTemperature(targetTemperature);

            var eta = DateTime.Now.AddMinutes(targetTemperature);
            var remaining = (eta - DateTime.Now);

            await _hubContext.Clients.All.SendAsync("UpdateETA", remaining);
        }
    }
}
