using System;

namespace Sauna.RPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var runner = new Runner();

                runner.TemperatureRead += OnTemperatureRead;

                runner.Start();

                Console.WriteLine($"Started {DateTime.Now.ToShortTimeString()}");

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void OnTemperatureRead(double internalTemperature, double externalTemperature)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} -> internal: {internalTemperature} C, external {externalTemperature} C");
        }
    }
}