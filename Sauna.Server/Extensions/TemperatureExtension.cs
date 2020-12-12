using UnitsNet;

namespace Sauna.Server.Extensions
{
    public static class TemperatureExtension
    {
        public static Sauna.Core.Temperature ToSaunaTemperature(this Temperature temperature)
        {
            return new Core.Temperature() { C = temperature.DegreesCelsius, F = temperature.DegreesFahrenheit, K = temperature.Kelvins };
        }
    }
}
