using System;

namespace Sauna.Console
{
    class Program
    {
        static Gui gui;
        static Controller.Controller _controller;

        static bool IsStarted;

        static void Main(string[] args)
        {
            _controller = new Controller.Controller();
            _controller.TemperatureRead += _controller_TemperatureRead;

            gui = new Gui();
            gui.Show();

            gui.Started += Gui_Started;

            _controller.Run();
            gui.Run();
        }

        static void _controller_TemperatureRead(int internalTemperature, int externalTemperature)
        {
            gui.UpdateTemperature(internalTemperature, externalTemperature, DateTime.Now);
        }

        static bool Gui_Started()
        {
            if (IsStarted)
            {
                Stop();
                IsStarted = false;

                return IsStarted;
            }

            Start();
            IsStarted = true;

            return IsStarted;
        }

        private static void Start()
        {
            _controller.TurnOn();
        }

        private static void Stop()
        {
            _controller.TurnOff();
        }
    }
}
