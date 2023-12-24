using Arayüz_Son.Model;
using Arayüz_Son.Services;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
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
    /// BMS_Sayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class BMS_Sayfa : Page
    {
        private Timer timer;
        private CancellationTokenSource _cancellationTokenSource;
        public BMS_Sayfa()
        {
            InitializeComponent();
            getBMSValue();
            //timer = new Timer(RefreshBMSValue, null, 0, 2000);
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RefreshBMSValue(_cancellationTokenSource.Token));
        }
        private void SendDataToFirebase(string path ,string value)
        {
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();

            var controlValues = value ;
            
            try
            {

                if (path == "Fan")
                {
                    FirebaseResponse pushResponce = client.Set(@"ControlValues/BMS/Fan", controlValues);
                    MessageBox.Show($"Firebase'deki {path} değeri güncellendi: {value}");                }
                else if (path == "Stop")
                {
                    FirebaseResponse pushResponce = client.Set(@"ControlValues/BMS/Stop", controlValues);
                    MessageBox.Show($"Firebase'deki {path} değeri güncellendi: {value}");
                }
                else if(path == "Light")
                {
                    FirebaseResponse pushResponce = client.Set(@"ControlValues/BMS/Light", controlValues);
                    MessageBox.Show($"Firebase'deki {path} değeri güncellendi: {value}");
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
        private async void RefreshBMSValue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    getBMSValue();
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

        public async void getBMSValue()
        {
            BatTem.Text = "";
            BatTem1.Content = "";
            BatTem2.Content = "";
            BatTem3.Content = "";
            BatTem4.Content = "";
            Batary.Content = "";
            CurrentVal.Content = "";
            TotalVol.Content = "";

            IFirebaseClient client = FirebaseConnect.Instance.GetClient();

            try
            {
                var responseTask = client.GetAsync(@"ControlValues/BMS");

                await Task.WhenAll(responseTask);

                var response = responseTask.Result;
                ControlValues controlValues = JsonConvert.DeserializeObject<ControlValues>(response.Body);
            
                if (controlValues != null )
                {
                    BatTem.Text = controlValues.BatTem.ToString();
                    BatTem1.Content = controlValues.BatTem1.ToString();
                    BatTem2.Content = controlValues.BatTem2.ToString();
                    BatTem3.Content = controlValues.BatTem3.ToString();
                    BatTem4.Content = controlValues.BatTem4.ToString();
                    Batary.Content = controlValues.Batary.ToString();
                    CurrentVal.Content = controlValues.CurrentVal.ToString();
                    TotalVol.Content = controlValues.TotalVol.ToString();
                }
                else
                {
                    Console.WriteLine("Deserialized object is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



        }

        private void Fan_On(object sender, RoutedEventArgs e)
        {
            string path = "Fan";
            string value = "on";
           SendDataToFirebase(path,value);
        }

        private  void Fan_Off(object sender, RoutedEventArgs e)
        {
            string path = "Fan";
            string value = "off";
            SendDataToFirebase(path, value);
        }

        private void Stop_On(object sender, RoutedEventArgs e)
        {
            string path = "Stop";
            string value = "on";
            SendDataToFirebase(path, value);
        }

        private void Stop_Off(object sender, RoutedEventArgs e)
        {
            string path = "Stop";
            string value = "off";
            SendDataToFirebase(path, value);
        }

        private void Light_On(object sender, RoutedEventArgs e)
        {
            string path = "Light";
            string value = "on";
            SendDataToFirebase(path, value);
        }

        private void Light_Off(object sender, RoutedEventArgs e)
        {
            string path = "Light";
            string value = "off";
            SendDataToFirebase(path, value);
        }
    }
}
