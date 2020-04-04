using Microsoft.AspNetCore.Components;
using Sauna.Core;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections;

namespace Sauna.Client.Pages
{
    public class MonitorComponent : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        #region UI Bindings

        internal double InternalTemperature { get; set; }
        internal double ExternalTemperature { get; set; }
        internal double TargetTemperature { get; set; }
        internal double TemperatureStep { get; }
        internal string ETA { get; set; }
        internal bool IsOn { get; set; }
        internal bool IsCalculating { get; set; }
        internal DateTime Now { get; set; }

        double MaxTemperature => 99;
        double MinTemperature => 70;

        #endregion

        HubConnection _connection;

        public MonitorComponent()
        {
            TargetTemperature = 80;
            TemperatureStep = 1;

            Now = DateTime.Now;
        }

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Initialized");

            _connection = new HubConnectionBuilder()
             .WithUrl(NavigationManager.ToAbsoluteUri("/saunaHub"), opt =>
             {
                 opt.Transports = HttpTransportType.WebSockets;
             })
             .Build();

            _connection.On<TemperatureReading>("NewTemperatureReading", OnTemperatureReading);
            _connection.On<bool>("UpdateSaunaStatus", UpdateSaunaStatus);
            _connection.On<TimeSpan>("UpdateETA", UpdateETA);
            _connection.Closed += OnConnectionClosed;

            await _connection.StartAsync();

            new Timer(Clock, null, 0, 1000);
        }

        Task OnConnectionClosed(Exception arg)
        {
            Console.WriteLine("Connection was closed! " + arg?.ToString());

            return Task.CompletedTask;
        }

        void Clock(object status)
        {
            Now = DateTime.Now;

            StateHasChanged();
        }

        Task UpdateETA(TimeSpan remaining)
        {
            ETA = ToPrettyFormat(remaining);

            IsCalculating = false;

            StateHasChanged();

            return Task.CompletedTask;
        }

        Task UpdateSaunaStatus(bool isOn)
        {
            IsOn = isOn;

            if (isOn)
            {
                _connection.InvokeAsync("EstimateETA", TargetTemperature);
            }

            StateHasChanged();

            return Task.CompletedTask;
        }

        Task OnTemperatureReading(TemperatureReading reading)
        {
            InternalTemperature = reading.Internal.C;
            ExternalTemperature = reading.External.C;

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

        internal async Task TemperatureUp()
        {
            if (TargetTemperature >= MaxTemperature)
            {
                return;
            }

            TargetTemperature += TemperatureStep;

            IsCalculating = true;
            await _connection.InvokeAsync("TemperatureUp", TargetTemperature);
            IsCalculating = false;
        }

        internal async Task TemperatureDown()
        {
            if (TargetTemperature <= MinTemperature)
            {
                return;
            }

            TargetTemperature -= TemperatureStep;

            IsCalculating = true;
            await _connection.InvokeAsync("TemperatureDown", TargetTemperature);
            IsCalculating = false;
        }

        internal string ToPrettyFormat(TimeSpan span)
        {
            if (span == TimeSpan.Zero)
            {
                return "0 minutes";
            }

            var sb = new StringBuilder();
            if (span.Days > 0)
            {
                _ = sb.AppendFormat("{0} day{1} ", span.Days, span.Days > 1 ? "s" : string.Empty);
            }

            if (span.Hours > 0)
            {
                _ = sb.AppendFormat("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : string.Empty);
            }

            if (span.Minutes > 0)
            {
                _ = sb.AppendFormat("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : string.Empty);
            }

            return sb.ToString();
        }
    }
}
