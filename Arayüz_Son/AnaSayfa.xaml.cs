using Arayüz_Son.Model;
using Arayüz_Son.Services;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Arayüz_Son
{
    /// <summary>
    /// AnaSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AnaSayfa : Page
    {
        //private Timer timer;
        IFirebaseClient client;
        private CancellationTokenSource _cancellationTokenSource;
        public AnaSayfa()
        {
            InitializeComponent();
            InitializeMap();
            speedValue();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RefreshSpeedValue(_cancellationTokenSource.Token));
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
                    await Task.Delay(TimeSpan.FromSeconds(1),cancellationToken);
                }catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
        private void speedValue()
        {
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            
            try
            {
                if (client == null)
                {
                    Console.WriteLine("Firebase client initialization failed.");
                    return;
                }
                FirebaseResponse response = client.Get(@"ControlValues");

                ControlValues controlValues = JsonConvert.DeserializeObject<ControlValues>(response.Body);

                if (controlValues != null)
                {
                    hız.Text = controlValues.Hız.ToString();
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

            gmapControl.Position = new GMap.NET.PointLatLng(39.481016, 29.900410);
            gmapControl.MinZoom = 1;
            gmapControl.MaxZoom = 20;
            gmapControl.Zoom = 15;
        }
    }
}
