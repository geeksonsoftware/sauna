using Microsoft.AspNetCore.SignalR;

namespace Sauna.RPI.Web
{
    class SaunaHub : Hub
    {
        readonly Messenger _messenger;

        public SaunaHub(Messenger messenger)
        {
            _messenger = messenger;
        }

        public string GetSaunaStatus()
        {
            return "off";
        }
    }
}