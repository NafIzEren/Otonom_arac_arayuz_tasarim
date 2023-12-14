using System.Windows;
using System.Windows.Controls;

namespace Arayüz_Son
{
    /// <summary>
    /// AraçSayfa.xaml etkileşim mantığı
    /// </summary>
    public partial class AraçSayfa : Page
    {
        public AraçSayfa()
        {
            InitializeComponent();
            InitializeMap();
        }
        private void InitializeMap()
        {
            gmapControl.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

            gmapControl.Position = new GMap.NET.PointLatLng(39.481016, 29.900410);
            gmapControl.MinZoom = 1;
            gmapControl.MaxZoom = 20;
            gmapControl.Zoom = 15;
        }

        private void Btn_Duscart(object sender, RoutedEventArgs e)
        {

        }
    }
}
