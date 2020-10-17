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
        public class NumeroInseridoEventArgs : EventArgs
        {
            public int NumeroIndice { get; set; }

            public NumeroInseridoEventArgs(int numeroIndice)
            {
                NumeroIndice = numeroIndice;
            }
        }

        public event EventHandler<NumeroInseridoEventArgs> NumeroInserido;

        public VendaCancelarItemInput()
        {
            InitializeComponent();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroItem = int.Parse(TextboxNumero.Text);
                NumeroInserido?.Invoke(this, new NumeroInseridoEventArgs(numeroItem));
                Close();
            }
            catch
            {
                MessageBox.Show("Numero inserido inválido.","Erro",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }
    }
}
