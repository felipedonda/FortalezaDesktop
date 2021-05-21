using FortalezaDesktop.Models;
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
    /// Interaction logic for PedidosEntrega.xaml
    /// </summary>
    public partial class PedidosEntrega : Window
    {
        Pedido Pedido {get; set;}
        public PedidosEntrega(Pedido pedido)
        {
            Pedido = pedido;
            InitializeComponent();
            DestacarStatus();
        }

        private void DestacarStatus()
        {
            ButtonStatus1.Background = Brushes.White;
            ButtonStatus2.Background = Brushes.White;
            ButtonStatus3.Background = Brushes.White;

            switch (Pedido.Status)
            {
                case 1:
                    ButtonStatus1.Background = Brushes.LightBlue;
                    break;
                case 2:
                    ButtonStatus2.Background = Brushes.LightBlue;
                    break;
                case 3:
                    ButtonStatus3.Background = Brushes.LightBlue;
                    break;
            }
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonStatus1_Click(object sender, RoutedEventArgs e)
        {
            Pedido.Status = 1;
            await Pedido.UpdateInstance();
            DestacarStatus();
        }
        private async void ButtonStatus2_Click(object sender, RoutedEventArgs e)
        {
            Pedido.Status = 2;
            await Pedido.UpdateInstance();
            DestacarStatus();
        }

        private async void ButtonStatus3_Click(object sender, RoutedEventArgs e)
        {
            Pedido.Status = 3;
            await Pedido.UpdateInstance();
            DestacarStatus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
