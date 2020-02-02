using Iot.Units;
using System;
using System.Threading;

namespace Sauna.RPI.Controlling
{
    public class SaunaController
    {
        ITemperatureReader _temperatureReader;

        Timer _updateTemperatureTimer;
        readonly TimeSpan _updateTemperatureInterval;

        public event Action<DateTime, Temperature, Temperature> TemperatureRead;

        public SaunaController()
        {
#if RPI
            _temperatureReader = new TemperatureReaderRPI();
#else
            _temperatureReader = new TemperatureReaderMock();
#endif

            _updateTemperatureInterval = TimeSpan.FromSeconds(10);
        }


        internal void Init()
        {
            _updateTemperatureTimer = new Timer(UpdateTemperature, null, _updateTemperatureInterval, _updateTemperatureInterval);

           // Console.WriteLine("Start monitoring the sauna");
        }

        void UpdateTemperature(object state)
        {
            try
            {
                var internalTemperature = _temperatureReader.ReadInternalTemperature();
                var externalTemperature = _temperatureReader.ReadExternalTemperature();

                if (internalTemperature.HasValue && externalTemperature.HasValue)
                {
                    TemperatureRead?.Invoke(DateTime.Now, internalTemperature.Value, externalTemperature.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}