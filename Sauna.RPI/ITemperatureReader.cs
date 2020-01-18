using Iot.Units;

namespace Sauna.RPI
{
    public interface ITemperatureReader
    {
        Temperature? ReadExternalTemperature();
        Temperature? ReadInternalTemperature();
    }
}