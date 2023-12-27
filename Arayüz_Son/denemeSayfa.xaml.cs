using Arayüz_Son.Model;
using Arayüz_Son.Services;
using FireSharp.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Arayüz_Son
{
    /// <summary>
    /// deneme_sayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class denemeSayfa : Page
    {
        //private Timer timer;
        IFirebaseClient client;
        private CancellationTokenSource _cancellationTokenSource;
        public denemeSayfa()
        {
            InitializeComponent();
            InitializeMap();
            speedValue();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RefreshSpeedValue(_cancellationTokenSource.Token));
            deneme();
        }

        FilterInfoCollection fico;
        VideoCaptureDevice vcd;

        private void deneme()
        {

            fico = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            vcd = new VideoCaptureDevice(fico[0].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();
        }

        private void BtnStart_click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Vcd_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
            {
                var bitmapImage = ConvertBitmapToBitmapImage(bitmap);
                Dispatcher.Invoke(() => { videoPlayer.Source = bitmapImage; });
            }
        }

        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Önemli: WPF'nin farklı thread'ler arası kullanımı için.
                return bitmapImage;
            }
        }

        private void denemeSayfa_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Unloaded");
            StopWebcam();
        }

        private void StopWebcam()
        {
            if (vcd != null && vcd.IsRunning)
            {
                vcd.SignalToStop();
                //vcd.Stop();
                Console.WriteLine("VCD");
                vcd.NewFrame -= new NewFrameEventHandler(Vcd_NewFrame);
                vcd = null; // Release the VideoCaptureDevice
            }
        }

        private async void RefreshSpeedValue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    speedValue();
                });
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
        private async void speedValue()
        {
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            myProgessBar.Value = 0;
            try
            {
                if (client == null)
                {
                    Console.WriteLine("Firebase client initialization failed.");
                    return;
                }
                var responseTask = client.GetAsync(@"ControlValues");
                var responseBmsTask = client.GetAsync(@"ControlValues/BMS");

                await Task.WhenAll(responseTask, responseBmsTask);

                var response = responseTask.Result;
                var responseBMS = responseBmsTask.Result;

                ControlValues controlValues = JsonConvert.DeserializeObject<ControlValues>(response.Body);
                ControlValues controlValuesBMS = JsonConvert.DeserializeObject<ControlValues>(responseBMS.Body);


                if (controlValues != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        hız.Text = controlValues.Hız.ToString();
                        int.TryParse(controlValuesBMS.Batary, out int bataryValue);
                        myProgessBar.Value = bataryValue;
                    });
                }
                else
                {
                    Console.WriteLine("Deserialized object is null.");
                }
            }
            catch (JsonSerializationException jsonEx)
            {
                Console.WriteLine("JSON Serialization Exception: " + jsonEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error during deserialization: " + ex.Message);
            }

        }
        private void InitializeMap()
        {
            gmapControl.MapProvider = GMap.NET.MapProviders.GoogleHybridMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

            gmapControl.Position = new GMap.NET.PointLatLng(40.789983, 29.508963);
            gmapControl.MinZoom = 1;
            gmapControl.MaxZoom = 19;
            gmapControl.Zoom = 19;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
