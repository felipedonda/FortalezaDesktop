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
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PontoVendaView.xaml
    /// </summary>
    public partial class PontoVendaView : Window
    {
        public PontoVendaView()
        {
            InitializeComponent();
        }

        public async Task LoadCaixas()
        {
            List<NomeCaixa> nomeCaixas = await new NomeCaixa().FindAll();
            ComboboxCaixa.ItemsSource = null;
            ComboboxCaixa.ItemsSource = nomeCaixas;
            ComboboxCaixa.DisplayMemberPath = "Nome";
            ComboboxCaixa.SelectedValuePath = "IdnomeCaixa";
            ComboboxCaixa.SelectedValue = UserPreferences.Preferences.IdnomeCaixa;
            if(nomeCaixas.Count < 2)
            {
                ButtonRemoverCaixa.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonRemoverCaixa.Visibility = Visibility.Visible;
            }
        }

        public async Task LoadPdvs()
        {
            List<Pdv> pdvs = await new Pdv().FindAll();
            ComboboxPDV.ItemsSource = null;
            ComboboxPDV.ItemsSource = pdvs;
            ComboboxPDV.DisplayMemberPath = "Nome";
            ComboboxPDV.SelectedValuePath = "Idpdv";
            ComboboxPDV.SelectedValue = UserPreferences.Preferences.Idpdv;
            if (pdvs.Count < 2)
            {
                ButtonRemoverPDV.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonRemoverPDV.Visibility = Visibility.Visible;
            }
        }

        private void ButtonNovoCaixa_Click(object sender, RoutedEventArgs e)
        {
            PontoVendaTextDialog pontoVendaTextDialog = new PontoVendaTextDialog("Nome do Caixa:");
            pontoVendaTextDialog.Submit += PontoVendaTextDialog_Submit_NovoCaixa;
            pontoVendaTextDialog.ShowDialog();
        }

        private async void PontoVendaTextDialog_Submit_NovoCaixa(object sender, PontoVendaTextDialog.SubmitEventArgs e)
        {
            NomeCaixa nomeCaixa = new NomeCaixa
            {
                Nome = int.Parse(e.Input)
            };
            await nomeCaixa.SaveInstance();
            await LoadCaixas();
        }

        private void ButtonAlterarCaixa_Click(object sender, RoutedEventArgs e)
        {
            PontoVendaTextDialog pontoVendaTextDialog = new PontoVendaTextDialog("Nome do Caixa:");
            pontoVendaTextDialog.LoadData(ComboboxCaixa.Text, (int)ComboboxCaixa.SelectedValue);
            pontoVendaTextDialog.Submit += PontoVendaTextDialog_Submit_AlterarCaixa;
            pontoVendaTextDialog.ShowDialog();
        }

        private async void PontoVendaTextDialog_Submit_AlterarCaixa(object sender, PontoVendaTextDialog.SubmitEventArgs e)
        {
            NomeCaixa nomeCaixa = new NomeCaixa
            {
                Nome = int.Parse(e.Input),
                IdnomeCaixa = e.Id
            };
            await nomeCaixa.UpdateInstance();
            await LoadCaixas();
        }

        private async void ButtonRemoverCaixa_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
            "Deseja realmente remover o caixa " + ComboboxCaixa.Text + "?",
            "Remover Caixa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NomeCaixa nomeCaixa = new NomeCaixa
                {
                    IdnomeCaixa = (int)ComboboxCaixa.SelectedValue
                };
                await nomeCaixa.DeleteInstance();
                await LoadCaixas();
            }
        }

        private async void ButtonRemoverPDV_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
            "Deseja realmente remover o ponto de venda " + ComboboxCaixa.Text + "?",
            "Remover PDV",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Pdv pdv = new Pdv
                {
                    Idpdv = (int)ComboboxPDV.SelectedValue
                };
                await pdv.DeleteInstance();
                await LoadCaixas();
            }
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            if(ComboboxCaixa.SelectedValue == null)
            {
                MessageBox.Show(
                    "Por favor selecione um Caixa.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            if (ComboboxPDV.SelectedValue == null)
            {
                MessageBox.Show(
                    "Por favor selecione um ponto de venda.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            UserPreferences.Preferences.IdnomeCaixa = (int)ComboboxCaixa.SelectedValue;
            UserPreferences.Preferences.Idpdv = (int)ComboboxPDV.SelectedValue;
            UserPreferences.Save();
            Close();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCaixas();
            await LoadPdvs();
        }

        private void ButtonNovoPDV_Click(object sender, RoutedEventArgs e)
        {
            PontoVendaTextDialog pontoVendaTextDialog = new PontoVendaTextDialog("Nome do ponto de venda:");
            pontoVendaTextDialog.Submit += PontoVendaTextDialog_Submit_NovoPDV;
            pontoVendaTextDialog.ShowDialog();
        }

        private async void PontoVendaTextDialog_Submit_NovoPDV(object sender, PontoVendaTextDialog.SubmitEventArgs e)
        {
            Pdv pdv = new Pdv
            {
                Nome = e.Input
            };
            await pdv.SaveInstance();
            await LoadPdvs();
        }

        private void ButtonAlterarPDV_Click(object sender, RoutedEventArgs e)
        {
            PontoVendaTextDialog pontoVendaTextDialog = new PontoVendaTextDialog("Nome do ponto de venda:");
            pontoVendaTextDialog.LoadData(ComboboxPDV.Text, (int)ComboboxPDV.SelectedValue);
            pontoVendaTextDialog.Submit += PontoVendaTextDialog_Submit_AlterarPDV;
            pontoVendaTextDialog.ShowDialog();
        }

        private async void PontoVendaTextDialog_Submit_AlterarPDV(object sender, PontoVendaTextDialog.SubmitEventArgs e)
        {
            Pdv pdv = new Pdv
            {
                Nome = e.Input,
                Idpdv = e.Id
            };
            await pdv.UpdateInstance();
            await LoadPdvs();
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
