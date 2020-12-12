using UnitsNet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sauna.RPI.Controlling
{
    public class SaunaController
    {
        ITemperatureReader _temperatureReader;

        double _targetTemperature;

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
        internal Task TurnOn()
        {
            return Task.CompletedTask;
        }

        internal Task TurnOff()
        {
            return Task.CompletedTask;
        }

        internal async Task UpdateTargetTemperature(double targetTemperature)
        {
            _targetTemperature = targetTemperature;

            //fake it takes a while to compute the new ETA
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}