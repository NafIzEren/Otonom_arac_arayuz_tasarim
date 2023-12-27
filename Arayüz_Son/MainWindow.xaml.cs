using HandyControl.Tools.Extension;
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
using System.Windows.Threading;

namespace Arayüz_Son
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        Color turuncu = Color.FromRgb(254, 93, 38);
        Color beyaz = Color.FromRgb(255, 250, 255);
        public MainWindow()
        {
            InitializeComponent();
            startclock();
        }

        private void startclock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            saat.Text = DateTime.Now.ToString(@"T");
            tarih.Text = DateTime.Now.ToString(@"D");
        }

        private void Btn_Duscart(object sender, RoutedEventArgs e)
        {
            Main.Content = new Duscart();
            Arac_Btn.Background = new SolidColorBrush(turuncu);
            Home_Btn.Background = new SolidColorBrush(turuncu);
            BMS_Btn.Background = new SolidColorBrush(turuncu);
        }

        private void Btn_Arac(object sender, RoutedEventArgs e)
        {
            Main.Content = new AraçSayfa();
            Arac_Btn.Background = new SolidColorBrush(beyaz);
            Home_Btn.Background = new SolidColorBrush(turuncu);
            BMS_Btn.Background = new SolidColorBrush(turuncu);
        }

        private void Btn_Home(object sender, RoutedEventArgs e)
        {
            Main.Content = new denemeSayfa();
            Arac_Btn.Background = new SolidColorBrush(turuncu);
            Home_Btn.Background = new SolidColorBrush(beyaz);
            BMS_Btn.Background = new SolidColorBrush(turuncu);
        }


        private void Btn_BMS(object sender, RoutedEventArgs e)
        {
            Main.Content = new BMS_Sayfa();
            Arac_Btn.Background = new SolidColorBrush(turuncu);
            Home_Btn.Background = new SolidColorBrush(turuncu);
            BMS_Btn.Background = new SolidColorBrush(beyaz);
        }

        private void Video_bitti(object sender, RoutedEventArgs e)
        {
            Main.Content = new denemeSayfa();
            Video.Hide();
            Home_Btn.Background = new SolidColorBrush(beyaz);
        }
    }
}
