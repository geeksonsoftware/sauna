using Iot.Device.DHTxx;
using Iot.Units;
using System;

namespace Sauna.RPI
{
    public class TemperatureReaderRPI : ITemperatureReader
    {
        readonly Dht22 _internalTemperature;
        readonly Dht22 _externalTemperature;

        public TemperatureReaderRPI()
        {
            _internalTemperature = new Dht22(15, System.Device.Gpio.PinNumberingScheme.Logical);
            _externalTemperature = new Dht22(18, System.Device.Gpio.PinNumberingScheme.Logical);
        }

        public Temperature? ReadInternalTemperature()
        {
            var temperature= _internalTemperature.Temperature;

            return _internalTemperature.IsLastReadSuccessful ? temperature: new Nullable<Temperature>();
        }

        public Temperature? ReadExternalTemperature()
        {
            var temperature = _externalTemperature.Temperature;

            return _externalTemperature.IsLastReadSuccessful ? temperature : new Nullable<Temperature>();
        }
    }
}
