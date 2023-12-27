using Arayüz_Son.Services;
using FireSharp.Interfaces;
using System;
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
        public AraçSayfa()
        {
            InitializeComponent();
            getCarPageVales();

        }

        private async void getCarPageVales()
        {
            horsePower.Text = "";
            torque.Text = "";
            IFirebaseClient client = FirebaseConnect.Instance.GetClient();
            try
            {



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

