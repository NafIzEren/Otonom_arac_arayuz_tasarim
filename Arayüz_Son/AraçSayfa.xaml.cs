﻿using Arayüz_Son.Model;
using Arayüz_Son.Services;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Swan.Formatters;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Arayüz_Son
{
    /// <summary>
    /// AraçSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AraçSayfa : Page
    {
        private CancellationTokenSource _cancellationTokenSource;
        public AraçSayfa()
        {
            InitializeComponent();
            getCarPageVales();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RefreshCarPageVales(_cancellationTokenSource.Token));

        }
        private async void RefreshCarPageVales(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    getCarPageVales();
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

        private async void getCarPageVales()
        {
            horsePower.Text = "";
            torque.Text = "";
            bas.Text = "";
            brake.Text = "";

            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            try
            {
                

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
                        bas.Text = controlValues.bas.ToString();
                        brake.Text = controlValues.brake.ToString();
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
                Console.WriteLine(ex.ToString());
            }

        }
        private void SendDataToFirebase(string value)
        {
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            try
            {
                FirebaseResponse firebase = client.Set(@"ControlValues/CarPage/OtoStart", value);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private bool isOn = true;

        private void BtnBasla_click(object sender, RoutedEventArgs e)
        {
            isOn = !isOn;
            if (isOn)
            {
                string value = "on";
                SendDataToFirebase(value);
            }
            else
            {
                string value = "off";
                SendDataToFirebase(value);

            }

            
        }

        private void Btn_Mod_1(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Mod_2(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Mod_3(object sender, RoutedEventArgs e)
        {

        }
    }
    public class WaveProgressBar : RangeBase
    {

    }
}

