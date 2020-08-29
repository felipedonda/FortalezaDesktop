using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for PagamentosVendaValorView.xaml
    /// </summary>
    public partial class VendaPagamentosValor : Window
    {
        public FormaPagamento MeioPagamento { get; set; }        
        public decimal Valor { get; set; }
        public event EventHandler InserirValor;

        public VendaPagamentosValor()
        {
            InitializeComponent();
        }

        public void LoadFormaPagamento(FormaPagamento meioPagamento)
        {
            MeioPagamento = meioPagamento;
            textblockTipoPagamento.Text = MeioPagamento.Nome;
        }

        public void SetValorRemanescente(decimal valor)
        {
            textblockValorPagamento.Text = valor.ToString("C2");
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Valor = decimal.Parse(textblockValorPagamento.Text, NumberStyles.Currency);
            InserirValor?.Invoke(this, new EventArgs());
            Close();
        }

        private void ButtonNumero_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textblockValorPagamento.Text))
                Limpar();
            Button senderAsButton = (Button)sender;
            TextBlock buttonText = (TextBlock)senderAsButton.Content;
            textblockValorPagamento.Text += buttonText.Text;
        }

        private void ButtonSeparator_Click(object sender, RoutedEventArgs e)
        {
            textblockValorPagamento.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            textblockValorPagamento.Text = textblockValorPagamento.Text.Substring(0, textblockValorPagamento.Text.Length -1);
        }

        public void Limpar()
        {
            textblockValorPagamento.Text = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + " ";
        }

        private void ButtonLimpar_Click(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
