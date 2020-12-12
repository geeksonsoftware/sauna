using UnitsNet;

namespace Sauna.RPI
{
    public interface ITemperatureReader
    {
        Temperature? ReadExternalTemperature();
        Temperature? ReadInternalTemperature();
    }
}