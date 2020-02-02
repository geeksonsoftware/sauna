using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sauna.Core;

namespace Sauna.Client.Pages
{
    public class MonitorComponent : ComponentBase
    {
        [Inject] private HttpClient _http { get; set; }
        [Inject] private HubConnectionBuilder _hubConnectionBuilder { get; set; }

        #region UI Bindings

        internal List<string> Messages { get; set; } = new List<string>();

        internal double InternalTemperature { get; set; }
        internal double ExternalTemperature { get; set; }

        #endregion

        HubConnection _connection;

        protected override async Task OnInitializedAsync()
        {
            _connection = _hubConnectionBuilder
                .WithUrl("/saunaHub",
                opt =>
                {
                    opt.LogLevel = SignalRLogLevel.None;
                    opt.Transport = HttpTransportType.WebSockets;
                    opt.SkipNegotiation = true;
                })
                //.AddMessagePackProtocol()
                .Build();

            _connection.On<string>("Send", Handle);
            _connection.On<TemperatureReading>("NewTemperatureReading", OnTemperatureReading);
            _connection.OnClose(exc =>
            {
                Console.WriteLine("Connection was closed! " + exc.ToString());

                return Task.CompletedTask;
            });
            await _connection.StartAsync();
        }

        Task OnTemperatureReading(TemperatureReading reading)
        {
            InternalTemperature = reading.Internal.C;
            ExternalTemperature = reading.External.C;

            StateHasChanged();

            return Task.CompletedTask;
        }

        // async Task<string> GetJwtToken(string userId)
        //{
        //    var httpResponse = await this._http.GetAsync($"generatetoken?user={userId}");
        //    httpResponse.EnsureSuccessStatusCode();

        //    return await httpResponse.Content.ReadAsStringAsync();
        //}

        Task Handle(string msg)
        {
            Console.WriteLine(msg);

            Messages.Add(msg.ToString());

            StateHasChanged();

            return Task.CompletedTask;
        }

        internal async Task TurnOn()
        {
            await _connection.InvokeAsync("TurnOn");
        }

        internal async Task TurnOff()
        {
            await _connection.InvokeAsync("TurnOff");
        }
    }
}
