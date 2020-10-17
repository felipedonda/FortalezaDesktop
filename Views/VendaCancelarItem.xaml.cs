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
    /// Interaction logic for VendaCancelarItem.xaml
    /// </summary>
    public partial class VendaCancelarItem : Window
    {
        public event EventHandler CancelarVenda;

        public event EventHandler<CancelarItemEventArgs> CancelarItem;

        public class CancelarItemEventArgs : EventArgs
        {
            public int IndiceItemVenda { get; set; }

            public CancelarItemEventArgs(int indiceItemVenda)
            {
                IndiceItemVenda = indiceItemVenda;
            }
        }

        public VendaCancelarItem()
        {
            InitializeComponent();
        }

        private void ButtonVenda_Click(object sender, RoutedEventArgs e)
        {
            CancelarVenda?.Invoke(this, new EventArgs());
            Close();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            VendaCancelarItemInput cancelarItemInput = new VendaCancelarItemInput();
            cancelarItemInput.NumeroInserido += CancelarItemInput_NumeroInserido;
            cancelarItemInput.Show();
        }

        private void CancelarItemInput_NumeroInserido(object sender, VendaCancelarItemInput.NumeroInseridoEventArgs e)
        {
            CancelarItem?.Invoke(this, new CancelarItemEventArgs(e.NumeroIndice));
            Close();
        }
    }
}
