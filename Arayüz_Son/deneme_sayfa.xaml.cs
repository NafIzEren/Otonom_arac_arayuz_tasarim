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

namespace Arayüz_Son
{
    /// <summary>
    /// deneme_sayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class deneme_sayfa : Page
    {
        //private Timer timer;
        IFirebaseClient client;
        private CancellationTokenSource _cancellationTokenSource;
        public deneme_sayfa()
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

            gmapControl.Position = new GMap.NET.PointLatLng(39.481016, 29.900410);
            gmapControl.MinZoom = 1;
            gmapControl.MaxZoom = 20;
            gmapControl.Zoom = 15;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
