using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for DeliveryImprimir.xaml
    /// </summary>
    public partial class DeliveryImprimir : Window
    {
        public event EventHandler CupomSelected;
        public event EventHandler CozinhaSelected;

        public DeliveryImprimir()
        {
            InitializeComponent();
        }

        private void buttonCupom_Click(object sender, RoutedEventArgs e)
        {
            CupomSelected?.Invoke(this, new EventArgs());
        }

        private void buttonCozinha_Click(object sender, RoutedEventArgs e)
        {
            CozinhaSelected?.Invoke(this, new EventArgs());
        }

        private void buttonFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
