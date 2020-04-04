using Iot.Device.DHTxx;
using Iot.Units;
using System;

namespace Sauna.RPI
{
    public class TemperatureReaderMock : ITemperatureReader
    {
        readonly Random _random;

        public TemperatureReaderMock()
        {
            _random = new Random();
        }

        public Temperature? ReadInternalTemperature()
        {
            return Temperature.FromCelsius(DateTime.Now.Millisecond % 50 + 50 );
        }

        public Temperature? ReadExternalTemperature()
        {
            return Temperature.FromCelsius(DateTime.Now.Millisecond % 2 + 20);
        }
    }
}
