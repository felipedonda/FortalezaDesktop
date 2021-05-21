using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public event EventHandler<BandeiraSelecionadaArgs> BandeiraSelecionada;
        public class BandeiraSelecionadaArgs : EventArgs
        {
            public Bandeira Bandeira { get; set; }
            public FormaPagamento FormaPagamento { get; set; }
            public BandeiraSelecionadaArgs(Bandeira bandeira, FormaPagamento formaPagamento)
            {
                Bandeira = bandeira;
                FormaPagamento = formaPagamento;
            }
        }

        public FormaPagamento FormaPagamento { get; set; }

        public VendaPagamentoBandeira(FormaPagamento formaPagamento)
        {
            InitializeComponent();
            FormaPagamento = formaPagamento;
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
            var bandeira = (Bandeira)senderAsButton.Tag;
            SelecionarBandeira(bandeira);

        }

        private void SelecionarBandeira(Bandeira bandeira)
        {
            BandeiraSelecionada?.Invoke(this, new BandeiraSelecionadaArgs(bandeira, FormaPagamento));
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.Escape):
                    Close();
                    break;
                case (Key.NumPad1):
                    var bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 1).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad2):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 2).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad3):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 3).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad4):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 4).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad5):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 5).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad6):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 6).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad7):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 7).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad8):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 8).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
                case (Key.NumPad9):
                    bandeira = ((List<Bandeira>)itemsBandeiras.ItemsSource).Where(e => e.Ordem == 9).FirstOrDefault();
                    if (bandeira != null)
                    {
                        SelecionarBandeira(bandeira);
                    }
                    break;
            }
        }
    }
}
