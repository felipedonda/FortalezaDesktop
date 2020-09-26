using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for VendaPagamentoBandeira.xaml
    /// </summary>
    public partial class VendaPagamentoBandeira : Window
    {
        public event EventHandler Selecionado;
        public Bandeira BandeiraSelecionada { get; set; }

        public VendaPagamentoBandeira()
        {
            InitializeComponent();
        }

        private async Task LoadBandeiras()
        {
            List<Bandeira> bandeiras = await new Bandeira().FindAll();
            itemsBandeiras.ItemsSource = null;
            itemsBandeiras.ItemsSource = bandeiras;
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBandeiras();
        }

        private void ButtonBandeira_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            BandeiraSelecionada = (Bandeira)senderAsButton.Tag;
            Selecionado?.Invoke(this, new EventArgs());
            Close();
        }
    }
}
