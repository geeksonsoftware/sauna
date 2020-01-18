using Iot.Device.DHTxx;
using Iot.Units;
using System;

namespace Sauna.RPI
{
    public class TemperatureReaderMock : ITemperatureReader
    {

        public TemperatureReaderMock()
        {
        }

        public Temperature? ReadInternalTemperature()
        {
            return Temperature.FromCelsius(50);
        }

        public Temperature? ReadExternalTemperature()
        {
            return Temperature.FromCelsius(20);
        }
    }
}
