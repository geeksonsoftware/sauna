using System.Collections.Generic;
using Microcharts;
using Xamarin.Forms;

namespace Sauna.UI
{
    public class CustomLineChartChart : Microcharts.Forms.ChartView
    {
        public CustomLineChartChart()
        {
            Chart = new LineChart();
        }
        public IEnumerable<ChartEntry> ChartEntries
        {
            get { return (IEnumerable<ChartEntry>)GetValue(ChartEntriesProperty); }
            set { SetValue(ChartEntriesProperty, value); }
        }

        public static readonly BindableProperty ChartEntriesProperty =
            BindableProperty.Create("ChartEntries",
                 typeof(IEnumerable<ChartEntry>),
                 typeof(IEnumerable<ChartEntry>),
                 null,
                 propertyChanged: HandleChartEntriesChanged);

        static void HandleChartEntriesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomLineChartChart)bindable).Chart.Entries = (IEnumerable<ChartEntry>)newValue;
        }
    }
}
