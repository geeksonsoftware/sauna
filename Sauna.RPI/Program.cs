using Sauna.RPI.Controlling;
using Sauna.RPI.Web;
using System.Threading.Tasks;

namespace Sauna.RPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var sauna = SaunaController.GetInstance();
            sauna.Init();

            var server = new SignalRSelfHost();
            await server.StartListenAsync();
        }
    }
}