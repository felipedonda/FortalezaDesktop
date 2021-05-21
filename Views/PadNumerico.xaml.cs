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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PadNumerico.xaml
    /// </summary>
    public partial class PadNumerico : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(decimal),
                typeof(PadNumerico),
                new FrameworkPropertyMetadata(
                    (decimal)0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(
                        (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                        {
                            PadNumerico padNumerico = obj as PadNumerico;
                            padNumerico.textblockValorPagamento.Text = ((decimal)e.NewValue).ToString("C2");
                        })
                    )
                );

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public event EventHandler OkClick;

        public event EventHandler CancelarClick;

        public PadNumerico()
        {
            InitializeComponent();
        }

        public void SetValorRemanescente(decimal valor)
        {
            Value = valor;
            textblockValorPagamento.Text = Value.ToString("C2");
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            OkClicked();
        }

        private void OkClicked()
        {
            try
            {
                Value = decimal.Parse(textblockValorPagamento.Text, NumberStyles.Currency);
            }
            catch
            {
                MessageBox.Show("Valor inserido inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OkClick?.Invoke(this, new EventArgs());
        }

        private void ButtonNumero_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textblockValorPagamento.Text))
            {
                Limpar();
            }
            Button senderAsButton = (Button)sender;
            textblockValorPagamento.Text += (string)senderAsButton.Tag;
        }

        private void ButtonSeparator_Click(object sender, RoutedEventArgs e)
        {
            if (textblockValorPagamento.Text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            {
                textblockValorPagamento.Text.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "");
            }
            textblockValorPagamento.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if(textblockValorPagamento.Text.Length > 0)
            {
                textblockValorPagamento.Text = textblockValorPagamento.Text[0..^1];
            }
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
            CancelarClick?.Invoke(this, new EventArgs());
        }

        private void ButtonNumeroSoma_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textblockValorPagamento.Text))
            {
                Limpar();
            }

            Button senderAsButton = (Button)sender;
            try
            {
                Value = decimal.Parse(textblockValorPagamento.Text, NumberStyles.Currency);
            }
            catch
            {
                Value = 0;
            }
            Value += decimal.Parse((string)senderAsButton.Tag);
            textblockValorPagamento.Text = Value.ToString("C2");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            Dispatcher.InvokeAsync(new Action(() =>
            {
                textblockValorPagamento.Focus();
                textblockValorPagamento.SelectAll();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void textblockValorPagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                OkClicked();
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                OkClicked();
            }

            if (e.Key == Key.F11)
            {
                CancelarClick?.Invoke(this, new EventArgs());
            }
        }
    }
}
