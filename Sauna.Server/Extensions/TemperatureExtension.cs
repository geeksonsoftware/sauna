namespace Sauna.Server.Extensions
{
    public static class TemperatureExtension
    {
        public static Sauna.Core.Temperature ToSaunaTemperature(this Iot.Units.Temperature temperature)
        {
            return new Core.Temperature() { C = temperature.Celsius, F = temperature.Fahrenheit, K = temperature.Kelvin };
        }
    }
}
