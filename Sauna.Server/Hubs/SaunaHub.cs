using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Sauna.Server.Hubs
{
    public class SaunaHub : Hub
    {    
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await this.Clients.Others.SendAsync("Send", $"{this.Context.ConnectionId} left");
        }
        
        public Task TurnOn()
        {
            return this.Clients.All.SendAsync("Send", $"Sauna is heating");
        }

        public Task TurnOff()
        {
            return this.Clients.All.SendAsync("Send", $"Sauna is off");
        }
    }
}
