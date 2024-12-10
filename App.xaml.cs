using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LiveChartV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Determine the architecture (x86 or x64)
            string arch = IntPtr.Size == 8 ? "x64" : "x86";
            string nativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arch);
            string libraryPath = Path.Combine(nativePath, "libSkiaSharp.dll");

            if (!File.Exists(libraryPath))
            {
                MessageBox.Show($"Native library not found: {libraryPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            // Load the library
            IntPtr libHandle = LoadLibrary(libraryPath);
            if (libHandle == IntPtr.Zero)
            {
                MessageBox.Show($"Failed to load native library: {libraryPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            // Test SkiaSharp initialization
            try
            {
                var info = new SkiaSharp.SKImageInfo(100, 100);
                Console.WriteLine($"SKImageInfo created: {info.Width}x{info.Height}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing SkiaSharp: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);
    }
}
