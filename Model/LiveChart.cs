using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using LiveChartsCore.Kernel;

namespace LiveChartV2.Model
{
    public class LiveChart
    {
        private const int MaxTimeSpanMinutes = 4;

        // Chart Data
        public ObservableCollection<DateTimePoint> Density { get; set; }

        // Series for the Chart
        public ISeries[] DensitySeries { get; set; }

        // Axes Configuration
        public Axis[] YAxes { get; set; }
        public Axis[] XAxes { get; set; }
        //public ISeries[] Series { get; set; } = new ISeries[]
        //{
        //    new ColumnSeries<int>(3, 4, 2),
        //    new ColumnSeries<int>(4, 2, 6),
        //    new ColumnSeries<double, DiamondGeometry>(4, 3, 4)
        //};

        public LiveChart()
        {
            Density = new ObservableCollection<DateTimePoint>();
            // Initialize Series for the Chart (LineSeries for Density)
            DensitySeries = new ISeries[]
            {
            new LineSeries<DateTimePoint>
            {
                Values = Density, // Bind the ObservableCollection
               Mapping = (point, index) => new Coordinate(point.DateTime.Ticks, point.Value),
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1 },
                Fill = null, // Transparent fill
                GeometrySize = 0 // Hide data points (optional)
            }
            };

            // Configure Axes
            XAxes = new Axis[]
            {
            new Axis
            {
                Labeler = value =>
        {
            try
            {
                // Safely convert the value to a DateTime
                return new DateTime((long)value).ToString("HH:mm:ss");
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Invalid Time";
            }
        },
                TextSize = 12
            }
            };

            YAxes = new Axis[]
            {
            new Axis
            {
                Labeler = value => value.ToString("0.00"),
                TextSize = 12
            }
            };
        }
        public void AddDensityDataPoint(DateTimePoint density)
        {
            if (density.DateTime.Ticks >= DateTime.MinValue.Ticks && density.DateTime.Ticks <= DateTime.MaxValue.Ticks)
            {
                Density.Add(density);

                if (Density.Count > 1)
                {
                    var timeSpan = density.DateTime - Density[0].DateTime;
                    if (timeSpan.TotalMinutes > MaxTimeSpanMinutes)
                    {
                        Density.RemoveAt(0);
                    }
                }
                // Trigger UI refresh
                DensitySeries = new ISeries[]
                {
            new LineSeries<DateTimePoint>
            {
                Values = Density,
                Mapping = (point, index) => new Coordinate(point.DateTime.Ticks, point.Value),
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1 },
                Fill = null, // Transparent fill
                GeometrySize = 0 // Hide data points (optional)
            }
                };
            }
        }
    }

}

public class DateTimePoint
{
    public DateTime DateTime { get; set; }
    public double Value { get; set; }

    public DateTimePoint(DateTime dateTime, double value)
    {
        DateTime = dateTime;
        Value = value;
    }
}