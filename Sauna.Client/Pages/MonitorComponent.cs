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

        internal double InternalTemperature { get; set; }
        internal double ExternalTemperature { get; set; }
        internal double TargetTemperature { get; set; }
        internal double TemperatureStep { get; }
        internal DateTime ETA { get; set; }

        double MaxTemperature => 99;
        double MinTemperature => 70;

        #endregion

        HubConnection _connection;

        public MonitorComponent()
        {
            TargetTemperature = 80;
            TemperatureStep = 1;
            ETA = DateTime.Now.AddHours(2);
        }

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
        
        internal async Task TurnOn()
        {
            await _connection.InvokeAsync("TurnOn");
        }

        internal async Task TurnOff()
        {
            await _connection.InvokeAsync("TurnOff");
        }

        internal async Task TemperatureUp()
        {
            if(TargetTemperature >= MaxTemperature)
            {
                return;
            }

            TargetTemperature += TemperatureStep;
            ComputeETA(1);
        }
        internal async Task TemperatureDown()
        {
            if(TargetTemperature<= MinTemperature)
            {
                return;
            }

            TargetTemperature -= TemperatureStep;
            ComputeETA(-1);
        }

        void ComputeETA(int upOrDown)
        {
            if (upOrDown > 0)
            {
                ETA = ETA.AddMinutes(10);
            }
            else
            {
                ETA = ETA.AddMinutes(-5);
            }
        }
    }
}
