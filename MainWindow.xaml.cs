using LiveChartsCore.Defaults;
using LiveChartV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LiveChartV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LiveChart liveChart { get; set; } = new LiveChart();
        private DispatcherTimer _updateTimer;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = liveChart;

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Update every second
            };
            _updateTimer.Tick += UpdateChart;
        }

        private void BtnMeasurement_Click(object sender, RoutedEventArgs e)
        {
            if (!_updateTimer.IsEnabled)
            {
                _updateTimer.Start(); // Start the timer on button click
            }
        }

        private void UpdateChart(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var newPoint = new DateTimePoint(DateTime.Now, CalculateDensityAverage());
                liveChart.AddDensityDataPoint(newPoint);
            });
        }

        private double CalculateDensityAverage()
        {
            // Example method to calculate density
            return new Random().NextDouble() * 10; // Replace with actual logic
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_updateTimer.IsEnabled)
            {
                _updateTimer.Stop(); // Stop the timer
            }
        }
    }
}
