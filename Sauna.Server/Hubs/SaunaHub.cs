using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Sauna.Server.Hubs
{
    public interface ISaunaHub
    {
        Task UpdateSaunaStatus(bool isOn);

        Task UpdateETA(DateTime eta);
    }
    public class SaunaHub : Hub<ISaunaHub>
    {
        readonly SaunaMessenger _messenger;

        public SaunaHub(SaunaMessenger messenger)
        {
            this._messenger = messenger;
        }
        //public override async Task OnDisconnectedAsync(Exception ex)
        //{
        //    await this.Clients.Others.SendAsync("Send", $"{this.Context.ConnectionId} left");
        //}
        
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

        public Task TemperatureUp(double targetTemperature)
        {
           return _messenger.UpdateTargetTemperature(targetTemperature);
        }

        public Task TemperatureDown(double targetTemperature)
        {
            return _messenger.UpdateTargetTemperature(targetTemperature);
        }
    }
}
