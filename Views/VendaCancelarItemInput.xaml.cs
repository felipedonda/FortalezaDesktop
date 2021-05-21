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
    /// Interaction logic for VendaCancelarItemInput.xaml
    /// </summary>
    public partial class VendaCancelarItemInput : Window
    {
        public bool SolicitarQuantidade { get; set; }
        public bool EsconderIndice { get; set; }

        public class NumeroInseridoEventArgs : EventArgs
        {
            public int NumeroIndice { get; set; }
            public decimal Quantidade { get; set; }


            public NumeroInseridoEventArgs(int numeroIndice, decimal quantidade)
            {
                NumeroIndice = numeroIndice;
                Quantidade = quantidade;
            }
        }

        public event EventHandler<NumeroInseridoEventArgs> NumeroInserido;

        public VendaCancelarItemInput(bool solicitarQuantidade = false, bool esconderIndice = false)
        {
            InitializeComponent();
            SolicitarQuantidade = solicitarQuantidade;
            EsconderIndice = esconderIndice;
            if (!SolicitarQuantidade)
            {
                TextblockQuantidade.Visibility = Visibility.Collapsed;
                TextboxQuantidade.Visibility = Visibility.Collapsed;
                TextboxQuantidade.IsTabStop = false;
            }
            if(EsconderIndice)
            {
                TextboxNumero.Visibility = Visibility.Collapsed;
                TextboxNumero.IsTabStop = false;
                TextblockNumero.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirmar()
        {
            int numeroItem = 0;
            int quantidade = 0;

            if (SolicitarQuantidade)
            {
                try
                {
                    quantidade = int.Parse(TextboxQuantidade.Text);
                }
                catch
                {
                    MessageBox.Show("Quantidade inserida inválida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (!EsconderIndice)
            {
                try
                {
                    numeroItem = int.Parse(TextboxNumero.Text);
                }
                catch
                {
                    MessageBox.Show("Numero inserido inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            NumeroInserido?.Invoke(this, new NumeroInseridoEventArgs(numeroItem, quantidade));
            Close();
        }

        private void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Confirmar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(!EsconderIndice)
            {
                TextboxNumero.Focus();
                TextboxNumero.SelectAll();
            }
            else
            {
                TextboxQuantidade.Focus();
                TextboxQuantidade.SelectAll();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirmar();
            }

            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
