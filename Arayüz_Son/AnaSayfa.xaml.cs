using Arayüz_Son.Model;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Arayüz_Son
{
    /// <summary>
    /// AnaSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AnaSayfa : Page
    {
        public AnaSayfa()
        {
            InitializeComponent();
            InitializeMap();
            FirebaseConnet();
            speedValue();
        }
        IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "iBmOzWiPc6jJSAcAoEbmHEujviHYKPEMjy3vI3sf",
            BasePath  = "https://duscartotonom-1419d-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        private void FirebaseConnet() 
        {
            try 
            {
                client = new FireSharp.FirebaseClient(fc);
               // MessageBox.Show("Baglandı!");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void speedValue()
        {
            hız.Text = "";

            FirebaseResponse response = client.Get(@"ControlValues");
            try
            {
                if (response == null || string.IsNullOrEmpty(response.Body))
                {
                    Console.WriteLine("Firebase response is null or empty.");
                    return;
                }

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
