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
