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
        public FormaPagamento FormaPagamento { get; set; }   
        public Bandeira Bandeira { get; set; }
        public event EventHandler<ValorInseridoArgs> InserirValor;
        public class ValorInseridoArgs : EventArgs
        {
            public FormaPagamento FormaPagamento { get; set; }
            public Bandeira Bandeira { get; set; }
            public decimal Valor { get; set; }
            public ValorInseridoArgs(decimal valor, FormaPagamento formaPagamento, Bandeira bandeira)
            {
                Valor = valor;
                FormaPagamento = formaPagamento;
                Bandeira = bandeira;
            }
        }

        public VendaPagamentosValor(FormaPagamento formaPagamento, Bandeira bandeira)
        {
            InitializeComponent();
            Bandeira = bandeira;
            FormaPagamento = formaPagamento;
            textblockTipoPagamento.Text = FormaPagamento.Nome;
            Title += FormaPagamento.Nome;
            PadNumerico.OkClick += PadNumerico_OkClick;
            PadNumerico.CancelarClick += PadNumerico_CancelarClick;
        }

        private void PadNumerico_CancelarClick(object sender, EventArgs e)
        {
            Close();
        }

        private void PadNumerico_OkClick(object sender, EventArgs e)
        {
            decimal valor = PadNumerico.Value;
            InserirValor?.Invoke(this, new ValorInseridoArgs(valor, FormaPagamento, Bandeira));
            Close();
        }

        public void SetValorRemanescente(decimal valor)
        {
            PadNumerico.Value = valor;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
