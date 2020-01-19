using Iot.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sauna.RPI
{
    class SaunaController
    {
        public Tuple<DateTime,Temperature,Temperature> LastRead { get; private set; }

        internal void Init()
        {
            try
            {
                var runner = new Runner();

                runner.TemperatureRead += OnTemperatureRead;

                runner.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var server = new Server();
                server.CommandReceived += OnCommandReceived;

                _ = server.StartListenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void OnCommandReceived(string command)
        {
            command = command.ToLower();

            switch (command)
            {
                case "get-temperature": break;
                case "start-sauna":break;
                case "stop-sauna":break;
            }
        }

        void OnTemperatureRead(DateTime readTime, Temperature internalTemperature, Temperature externalTemperature)
        {
            LastRead = new Tuple<DateTime, Temperature, Temperature>(readTime, internalTemperature, externalTemperature);

            Console.WriteLine($"{readTime.ToLongTimeString()} -> internal: {internalTemperature.Celsius} C, external {externalTemperature.Celsius} C");
        }
    }
}