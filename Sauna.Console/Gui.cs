

using System;
using Terminal.Gui;

namespace Sauna.Console
{
    class Gui
    {
        const int ColumnLabelWidth = 8;
        const int ColumnValuesWidth = 5;
        const int ColumnLabelStartPosition = 3;

        bool _isStarted;

        Label _temperatureInternalValue;
        Label _temperatureExternalValue;
        Label _etaValue;
        ProgressBar _progressBar;
        Button _startButton;

        TextField _desiredTemperatureValue;

        public event Func<bool> Started;

        public void Show()
        {
            Application.Init();
            var top = Application.Top;

            // Creates the top-level window to show
            var win = new Window("Sauna")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                }),
            });
            top.Add(menu);

            var temperatureInternalLabel = new Label("T in: ".PadRight(ColumnLabelWidth)) { X = ColumnLabelStartPosition, Y = 2 };
            _temperatureInternalValue = new Label("") { X = Pos.Right(temperatureInternalLabel), Y = Pos.Top(temperatureInternalLabel), Width = ColumnValuesWidth };

            var temperatureExternalLabel = new Label("T out: ".PadRight(ColumnLabelWidth)) { X = ColumnLabelStartPosition, Y = 3 };
            _temperatureExternalValue = new Label("") { X = Pos.Right(temperatureExternalLabel), Y = Pos.Top(temperatureExternalLabel), Width = ColumnValuesWidth };

            var etaLabel = new Label("ETA: ".PadRight(ColumnLabelWidth)) { X = ColumnLabelStartPosition, Y = 9 };
            _etaValue = new Label("") { X = Pos.Right(etaLabel), Y = Pos.Top(etaLabel), Width = ColumnValuesWidth };

            _progressBar = new ProgressBar() { X = ColumnLabelStartPosition, Y = 10, Width = win.Width - ColumnLabelStartPosition };

            var desiredTemperatureLabel = new Label("Desired temperature: ") { X = ColumnLabelStartPosition, Y = 7 };
            _desiredTemperatureValue = new TextField("70") { X = Pos.Right(desiredTemperatureLabel), Y = Pos.Top(desiredTemperatureLabel), Width = 3 };

            win.Add(
                desiredTemperatureLabel,
                _desiredTemperatureValue,
                new Button(3, 3, " + "),
                new Button(3, 3, " - ")
            );

            _startButton = new Button("Start", true)
            {
                X = ColumnLabelStartPosition,
                Y = 24,
                Clicked = StartButtonClicked
            };

            win.Add(
                temperatureInternalLabel, _temperatureInternalValue,
                temperatureExternalLabel, _temperatureExternalValue,
                etaLabel, _etaValue, _progressBar,
                _startButton
            );
        }

        internal void Run()
        {
            Application.Run();
        }
        internal bool Quit()
        {
            return true;
        }

        internal void StartButtonClicked()
        {
            _isStarted = Started?.Invoke() ?? false;

            _startButton.Text = _isStarted ? "Stop" : "Start";
        }

        internal void UpdateTemperature(int temperatureInternal, int temperatureExternal, DateTime eta)
        {
            Application.MainLoop.Invoke(() =>
            {
                _temperatureInternalValue.Text = (temperatureInternal.ToString() + " C").PadLeft(ColumnValuesWidth);
                _temperatureExternalValue.Text = (temperatureExternal.ToString() + " C").PadLeft(ColumnValuesWidth);

                if (!_isStarted)
                {
                    return;
                }

                var desiredTemperature = float.Parse(_desiredTemperatureValue.Text.ToString());
                if (temperatureInternal >= desiredTemperature)
                {
                    _etaValue.Text = "Ready!!!";
                    _progressBar.Fraction = 1;
                }
                else
                {
                    _etaValue.Text = (eta.ToString("HH:mm")).PadLeft(ColumnValuesWidth);

                    _progressBar.Fraction = temperatureInternal / desiredTemperature;
                }
            });
        }
    }
}
