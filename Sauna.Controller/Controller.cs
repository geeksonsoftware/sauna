using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sauna.Controller
{
    public class Controller
    {
        public bool IsHeating { get; private set; }

        public delegate void NewTemperatureReading(int internalTemperature, int externalTemperature);

        public event NewTemperatureReading TemperatureRead;
        public void TurnOn()
        {
            IsHeating = true;
        }

        public void TurnOff()
        {
            IsHeating = false;
        }

        public void Run()
        {
            var temperatureTask = Task.Run(async () =>
            {
                var rpi = new Raspberry();

                while (true)
                {
                    var internalTemperature = rpi.ReadInternalTemperature();
                    var externalTemperature=rpi.ReadExternalTemperature();

                    try
                    {
                        TemperatureRead?.Invoke(internalTemperature, externalTemperature);
                    }
                    catch { }

                    await Task.Delay(1000);
                }
            });
        }
    }
}
