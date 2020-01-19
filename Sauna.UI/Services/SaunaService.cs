using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sauna.UI.Services
{
    public class SaunaService
    {
        private HubConnection _connection;

        public event EventHandler<TemperatureHumidityEventArgs> NewInternalData;
        public event EventHandler<TemperatureHumidityEventArgs> NewExternalData;

        public SaunaService()
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:53353/signalr")
                    .Build();
        }
        public async Task<bool> Connect() {
            await _connection.StartAsync();
            _connection.On<string, string, string>("NewData", (type, temperatureCelsius, humidity) =>
            {
                var newData = new TemperatureHumidityEventArgs
                {
                    Temperature = float.Parse(temperatureCelsius, CultureInfo.InvariantCulture.NumberFormat),
                    Humidity = float.Parse(humidity, CultureInfo.InvariantCulture.NumberFormat),
                };

                if (type.ToLower().Equals("internal"))
                {
                    NewInternalData?.Invoke(this, newData);
                }
                else
                {
                    NewExternalData?.Invoke(this, newData);
                }
            });
            return true;
        }

        public async Task SendAction(SaunaAction action) {
            var method = action == SaunaAction.Start ? "Start" : "Stop";
            await _connection.InvokeAsync(method);
        }
    }
}
