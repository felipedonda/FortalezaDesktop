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
    /// Interaction logic for PontoVendaTextDialog.xaml
    /// </summary>
    public partial class PontoVendaTextDialog : Window
    {
        public int Id { get; set; }

        public class SubmitEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Input { get; set; }
            public bool IsNew { get; set; }

            public SubmitEventArgs (int id, string input, bool isNew)
            {
                Id = id;
                Input = input;
                IsNew = isNew;
            }
        }

        public event EventHandler<SubmitEventArgs> Submit;

        public PontoVendaTextDialog(string dialog)
        {
            InitializeComponent();
            Id = -1;
            TextboxDialogo.Text = dialog;
            ButtonAlterar.Visibility = Visibility.Collapsed;
        }

        public void LoadData(string input, int id)
        {
            ButtonAlterar.Visibility = Visibility.Visible;
            ButtonCriar.Visibility = Visibility.Collapsed;
            TextboxInput.Text = input;
            Id = id;
            
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAlterar_Click(object sender, RoutedEventArgs e)
        {
            Submit?.Invoke(this, new SubmitEventArgs(Id, TextboxInput.Text, false));
            Close();
        }

        private void ButtonCriar_Click(object sender, RoutedEventArgs e)
        {
            Submit?.Invoke(this, new SubmitEventArgs(Id, TextboxInput.Text, true));
            Close();
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
