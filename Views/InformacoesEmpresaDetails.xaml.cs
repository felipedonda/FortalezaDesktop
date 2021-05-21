using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using FortalezaDesktop.Utils;
using Microsoft.Win32;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for InformacoesEmpresaDetails.xaml
    /// </summary>
    public partial class InformacoesEmpresaDetails : Window
    {
        public InformacoesEmpresa InformacoesEmpresa { get; set; }
        private bool IsNew { get; set; }
        private bool UploadLogo { get; set; }
        private MemoryStream Logo { get; set; }
        private string LogoFileName { get; set; }
        private bool SalvarInProgress { get; set; }

        public InformacoesEmpresaDetails()
        {
            InitializeComponent();
            textblockErroCpf.Visibility = Visibility.Hidden;
            textblockErroRG.Visibility = Visibility.Hidden;
            TextblockLogradouroErro.Visibility = Visibility.Hidden;
            TextblockNumeroErro.Visibility = Visibility.Hidden;
            IsNew = false;
            UploadLogo = false;
            SalvarInProgress = false;
            Closing += InformacoesEmpresaDetails_Closing;
        }

        private async void InformacoesEmpresaDetails_Closing(object sender, CancelEventArgs e)
        {
            if(Logo != null)
            {
                await Logo.DisposeAsync();
            }
        }

        public async Task<bool> ValidateInformacoes()
        {
            textblockErroCpf.Visibility = Visibility.Hidden;
            textblockErroRG.Visibility = Visibility.Hidden;
            TextblockLogradouroErro.Visibility = Visibility.Hidden;
            TextblockNumeroErro.Visibility = Visibility.Hidden;
            bool validated = true;

            if (InformacoesEmpresa.Cpf != null)
            {
                if (InformacoesEmpresa.Cpf.Length != 11 & InformacoesEmpresa.Cpf.Length != 14)
                {
                    textblockErroCpf.Text = "CPF ou CNPJ inválido.";
                    textblockErroCpf.Visibility = Visibility.Visible;
                    validated = false;
                }
            }
            else
            {
                textblockErroCpf.Text = "Por favor inserir o CPF ou o CNPJ.";
                textblockErroCpf.Visibility = Visibility.Visible;
                validated = false;
            }

            if (InformacoesEmpresa.InscricaoEstadual != null)
            {
                if (InformacoesEmpresa.InscricaoEstadual.Length != 9 & InformacoesEmpresa.InscricaoEstadual.Length != 12)
                {
                    textblockErroRG.Text = "RG ou IE inválido.";
                    textblockErroRG.Visibility = Visibility.Visible;
                    validated = false;
                }
            }
            else
            {
                textblockErroRG.Text = "Por favor inserir o RG ou a IE.";
                textblockErroRG.Visibility = Visibility.Visible;
                validated = false;
            }

            if (string.IsNullOrEmpty(TextboxLogradouro.Text))
            {
                TextblockLogradouroErro.Text = "Logradouro vazio.";
                TextblockLogradouroErro.Visibility = Visibility.Visible;
                validated = false;
            }

            if (string.IsNullOrEmpty(TextboxNumero.Text))
            {
                TextblockNumeroErro.Text = "Número vazio.";
                TextblockNumeroErro.Visibility = Visibility.Visible;
                validated = false;
            }

            return validated;
        }

        private async void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            if(SalvarInProgress == true)
            {
                return;
            }

            SalvarInProgress = true;

            if(await ValidateInformacoes())
            {
                if (UploadLogo)
                {
                    if(await InformacoesEmpresa.UploadLogo(Logo, LogoFileName))
                    {
                        await Logo.DisposeAsync();
                    }
                }

                if (IsNew)
                {
                    if(await InformacoesEmpresa.SaveInstance())
                    {
                        Close();
                    }
                }
                else
                {
                    if(await InformacoesEmpresa.UpdateInstance())
                    {
                        Close();
                    }
                }
            }
            SalvarInProgress = false;
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxRegimeTributario.ItemsSource = new List<string>
            {
                "Simples Nacional",
                "Lucro Presumido",
                "Lucro Real"
            };
            await LoadInformacoesEmpresa();
        }

        public async Task LoadInformacoesEmpresa()
        {
            InformacoesEmpresa = await new InformacoesEmpresa().FindById(1);
            if (InformacoesEmpresa == null)
            {
                IsNew = true;
                InformacoesEmpresa = new InformacoesEmpresa
                {
                    IdenderecoNavigation = new Endereco()
                };
            }
            GridInformacoes.DataContext = InformacoesEmpresa;
          
            if(InformacoesEmpresa.Logo != null)
            {
                Stream image = await InformacoesEmpresa.GetImage();
                if(image != null)
                {
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = image;
                    imageSource.EndInit();
                    ImageLogo.Source = imageSource;
                    await image.DisposeAsync();
                }
            }
            if(InformacoesEmpresa.RegimeTributario != 0)
            {
                TextboxCsosn.Text = "";
                TextboxCsosn.IsEnabled = false;
            }
        }

        public async Task ReloadInformacoesEmpresa()
        {
            GridInformacoes.DataContext = null;
            GridInformacoes.DataContext = InformacoesEmpresa;
        }

        private async void buttonConsultarCEP(object sender, RoutedEventArgs e)
        {
            try
            {
                InformacoesEmpresa.IdenderecoNavigation = await ServicoCEP.ConsultarCEP(textboxCep.Text, InformacoesEmpresa.IdenderecoNavigation);
                await ReloadInformacoesEmpresa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void ButtonCarregarLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                RestoreDirectory = true,
                Title = "Selecionar logo da empresa",
                Filter = "Image Files(*.JPG;*.PNG;*.BMP)|*.JPG;*.PNG;*.BMP|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() ?? default)
            {
                using Stream fileStream = openFileDialog.OpenFile();
                LogoFileName = openFileDialog.FileName;
                Logo = new MemoryStream();
                await fileStream.CopyToAsync(Logo);
                fileStream.Dispose();

                try
                {
                    Logo.Seek(0, SeekOrigin.Begin);
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = Logo;
                    imageSource.EndInit();
                    ImageLogo.Source = imageSource;

                    UploadLogo = true;
                }
                catch(Exception ex)
                {
                    await Logo.DisposeAsync();
                    MessageBox.Show(ex.Message + "\n\nStack:\n" + ex.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void ButtonLimparLogo_Click(object sender, RoutedEventArgs e)
        {
            InformacoesEmpresa.Logo = null;
            ImageLogo.Source = null;
            UploadLogo = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void ComboboxRegimeTributario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InformacoesEmpresa.RegimeTributario != 0)
            {
                TextboxCsosn.IsEnabled = false;
            }
            else
            {
                TextboxCsosn.IsEnabled = true;
            }
        }
    }
}
