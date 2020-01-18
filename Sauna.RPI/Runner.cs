using System;
using System.Threading.Tasks;

namespace Sauna.RPI
{
    internal class Runner
    {
        ITemperatureReader _temperatureReader;

        public event Action<double, double> TemperatureRead;

        public Runner()
        {
#if RPI
            _temperatureReader = new TemperatureReaderRPI();
#else
            _temperatureReader = new TemperatureReaderMock();
#endif
        }

        internal void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        var internalTemperature = _temperatureReader.ReadInternalTemperature();
                        var externalTemperature = _temperatureReader.ReadExternalTemperature();

                        if (internalTemperature.HasValue && externalTemperature.HasValue)
                        {
                            TemperatureRead?.Invoke(internalTemperature.Value.Celsius, externalTemperature.Value.Celsius);
                        }

                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }).ConfigureAwait(false);
        }
    }
}