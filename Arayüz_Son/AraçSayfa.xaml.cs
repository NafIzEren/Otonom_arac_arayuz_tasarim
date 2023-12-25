using System.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Arayüz_Son.Model;
using Arayüz_Son.Services;
using FireSharp.Interfaces;
using HelixToolkit.Wpf;
using Newtonsoft.Json;

namespace Arayüz_Son
{
    /// <summary>
    /// AraçSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AraçSayfa : Page
    {
        IFirebaseClient client;
        private CancellationTokenSource _cancellationTokenSource;
        public AraçSayfa()
        {
            InitializeComponent();
            getValue();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RefreshValue(_cancellationTokenSource.Token));

        }

        private async void RefreshValue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    getValue();
                });
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public async void getValue()
        {
            horsePower.Text = "";
            torque.Text = "";
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            try
            {
                if (client == null)
                {
                    Console.WriteLine("Firebase client initialization failed.");
                    return;
                }
                var responseTask = client.GetAsync(@"ControlValues/CarPage");

                await Task.WhenAll(responseTask);

                var response = responseTask.Result;
        
                ControlValues controlValues = JsonConvert.DeserializeObject<ControlValues>(response.Body);


                if (controlValues != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        horsePower.Text = controlValues.horsePower.ToString();
                        torque.Text = controlValues.torque.ToString();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class WaveProgressBar : RangeBase
    {

    }
}

