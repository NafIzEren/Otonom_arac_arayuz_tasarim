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
            Main.Content = new AnaSayfa();
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
    }
}
