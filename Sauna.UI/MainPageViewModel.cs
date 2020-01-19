using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Microcharts;
using Sauna.UI.Services;
using SkiaSharp;
using Xamarin.Forms;

namespace Sauna.UI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int targetTemperature = 80;
        public int TargetTemperature
        {
            set
            {
                targetTemperature = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetTemperature)));
            }
            get
            {
                return targetTemperature;
            }
        }

        string currentStatus;
        public string CurrentStatus
        {
            set
            {
                currentStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStatus)));
            }
            get
            {
                return currentStatus;
            }
        }

        public ICommand StartCommand { private set; get; }
        public ICommand StopCommand { private set; get; }
        public bool IsHeating { get; private set; }
        
        public Chart HeatingChart { get; private set; }

        private SaunaService _saunaService;

        public MainPageViewModel()
        {
            HeatingChart = new LineChart()
            {
                Entries = new List<ChartEntry> { },
                MinValue = 0.0F,
                MaxValue = 100.0F,
                Margin = 10,
            };

            _saunaService = new SaunaService();

            HeatingChart.IsAnimated = false;
            StartCommand = new Command(
                execute: async () =>
                {
                    try
                    {
                        IsHeating = true;
                        await _saunaService.Connect();
                        await _saunaService.SendAction(SaunaAction.Start);
                        CurrentStatus = "Heating";
                        _saunaService.NewInternalData += (e, args) =>
                        {
                            var msg = $"New internal data: {args.Temperature}°C, {args.Humidity}%";
                            Console.WriteLine(msg);
                            CurrentStatus = msg;
                        };
                        _saunaService.NewExternalData += (e, args) =>
                        {
                            var msg = $"New external data: {args.Temperature}°C, {args.Humidity}%";
                            Console.WriteLine(msg);
                            CurrentStatus = msg;
                        };
                        /*foreach (int index in Enumerable.Range(1, 10))
                        {
                            var newValue = new Random().Next(100);
                            var newEntry = new ChartEntry(newValue);// { Label="WTF", ValueLabel = newValue.ToString() };
                            (HeatingChart.Entries as List<ChartEntry>).Add(newEntry);
                            MethodInfo dynMethod = HeatingChart.GetType().GetMethod("Invalidate",
                            BindingFlags.NonPublic | BindingFlags.Instance);
                            dynMethod.Invoke(HeatingChart, new object[] { });
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeatingChart)));
                            await Task.Delay(1000);
                        }*/
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                        IsHeating = false;
                    }
                },
                canExecute: () =>
                {
                    return !IsHeating;
                });

            StopCommand = new Command(
               execute: async () =>
               {
                   CurrentStatus = "Stopping Heating";
                   await _saunaService.Connect();
                   await _saunaService.SendAction(SaunaAction.Stop);
                   CurrentStatus = "Stopped Heating";
                   IsHeating = false;
               },
               canExecute: () =>
               {
                   return IsHeating;
               });
        }
    }
}
