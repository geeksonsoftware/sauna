using Iot.Units;
using Microsoft.AspNetCore.SignalR;
using Sauna.Core;
using Sauna.RPI.Controlling;
using System;
using Sauna.Server.Extensions;

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
    }
}
