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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for TrocaView.xaml
    /// </summary>
    public partial class TrocaView : Page
    {
        public TrocaView()
        {
            InitializeComponent();
            SalvandoTroca = false;
        }

        public Venda Venda { get; set; }
        public Troca Troca { get; set; }
        public bool SalvandoTroca { get; set; }

        private async void ButtonBuscar_Click(object sender, RoutedEventArgs e)
        {
            //int idvenda = int.Parse(textboxCodigo.Text);
            //await LoadVenda(idvenda);
            BuscaVenda buscaVenda = new BuscaVenda();
            buscaVenda.VendaSelecionada += BuscaVenda_VendaSelecionada;
            buscaVenda.ShowDialog();
        }

        private async void BuscaVenda_VendaSelecionada(object sender, BuscaVenda.VendaSelecionadaArgs e)
        {
            await LoadVenda(e.Venda.Idvenda);
        }

        public async Task LoadVenda(int idvenda)
        {
            Venda venda = await new Venda().FindById(idvenda, new Dictionary<string, string> {
                { "pagamentos", "true" },
                {"itemvendas", "true" },
                {"troca","true" }
            });

            if (venda != null)
            {
                if (venda.Troca != null)
                {
                    Troca = venda.Troca;
                    ReloadTroca();
                }
            }

            await LoadVenda(venda);
        }

        public async Task LoadVenda(Venda venda)
        {
            Venda = venda;
            MainGrid.DataContext = Venda;
        }

        public void ReloadVenda()
        {
            MainGrid.DataContext = null;
            MainGrid.DataContext = Venda;
        }

        private void ButtonSelecionarItem_Click(object sender, RoutedEventArgs e)
        {
            VendaCancelarItemInput cancelarItemInput = new VendaCancelarItemInput(true);
            cancelarItemInput.NumeroInserido += CancelarItemInput_NumeroInserido_Selecionar;
            cancelarItemInput.ShowDialog();
        }

        private void CancelarItemInput_NumeroInserido_Selecionar(object sender, VendaCancelarItemInput.NumeroInseridoEventArgs e)
        {
            foreach(var itemVenda in Venda.ItemVenda)
            {
                if(itemVenda.Indice == e.NumeroIndice)
                {
                    if(itemVenda.QuantidadeDisponivel >= e.Quantidade)
                    {
                        itemVenda.Cancelado += e.Quantidade;
                        AddItemTroca(itemVenda, e.Quantidade);
                    }
                    else
                    {
                        MessageBox.Show("Quantidade indisponível.", "Troca", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        public void ReloadTroca()
        {
            gridItemsTroca.ItemsSource = null;
            TextblockValorTroca.Text = "";
            if (Troca != null)
            {
                if(Troca.TrocaHasItemVenda != null)
                {
                    gridItemsTroca.ItemsSource = Troca.TrocaHasItemVenda;
                    TextblockValorTroca.Text = Troca.ValorTotal.ToString("C2");
                }
            }

        }

        private void AddItemTroca(ItemVenda item, decimal quantidade)
        {
            if(Troca == null)
            {
                Troca = new Troca
                {
                    Hora = DateTime.Now,
                    Idpdv = UserPreferences.Preferences.Idpdv,
                    Idusuario = UserController.UsuarioLogado.Idusuario,
                    Idvenda = Venda.Idvenda
                };
            }

            Troca.AddItemVenda(item, quantidade);

            ReloadTroca();
            ReloadVenda();
        }

        private void ButtonCancelarItem_Click(object sender, RoutedEventArgs e)
        {
            VendaCancelarItemInput cancelarItemInput = new VendaCancelarItemInput(false);
            cancelarItemInput.NumeroInserido += CancelarItemInput_NumeroInserido_CancelarTrocar;
            cancelarItemInput.ShowDialog();
        }

        private void CancelarItemInput_NumeroInserido_CancelarTrocar(object sender, VendaCancelarItemInput.NumeroInseridoEventArgs e)
        {
            bool encontrado = false;
            TrocaHasItemVenda itemTrocaSelecionado = null;
            if(Troca != null)
            {
                if(Troca.TrocaHasItemVenda !=null)
                {
                    foreach (var itemTroca in Troca.TrocaHasItemVenda)
                    {
                        if (itemTroca.Indice == e.NumeroIndice)
                        {
                            foreach (var itemVenda in Venda.ItemVenda)
                            {
                                if (itemVenda.IditemVenda == itemTroca.IditemVenda)
                                {
                                    itemTrocaSelecionado = itemTroca;
                                    encontrado = true;
                                    itemVenda.Cancelado = 0;
                                }
                            }
                        }
                    }

                    if (encontrado)
                    {
                        if(itemTrocaSelecionado != null)
                        {
                            Troca.TrocaHasItemVenda.Remove(itemTrocaSelecionado);
                            ReloadTroca();
                            ReloadVenda();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item inserido inválido.", "Troca", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum item para cancelar.", "Troca", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Nenhum item para cancelar.", "Troca", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private bool ValidateTroca()
        {
            if(Troca.TrocaHasItemVenda.Count < 1)
            {
                MessageBox.Show("Nenhum item selecionado para troca.", "Troca", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if(Venda.IdclienteNavigation == null)
            {
                MessageBox.Show("Por favor, selecionar ou cadastrar o cliente.", "Troca", MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        private async void ButtonConcluir_Click(object sender, RoutedEventArgs e)
        {
            if(!SalvandoTroca)
            {
                SalvandoTroca = true;
                if (ValidateTroca())
                {
                    if (await Troca.SaveInstance())
                    {
                        MessageBox.Show("Troca concluída com sucesso.", "Troca", MessageBoxButton.OK);
                        Limpar();
                    }
                    else
                    {
                        SalvandoTroca = false;
                    }
                }
                else
                {
                    SalvandoTroca = false;
                    return;
                }
            }
            
        }

        public void Limpar()
        {
            Venda = null;
            Troca = null;
            ReloadVenda();
            ReloadTroca();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private void ButtonCliente_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsCliente detailsCliente = new PedidoDetailsCliente();
            detailsCliente.Selecionado += DetailsCliente_Selecionado;
            detailsCliente.ShowDialog();
        }

        private async void DetailsCliente_Selecionado(object sender, PedidoDetailsCliente.ClienteSelecionadoEventArgs e)
        {
            Venda.Idcliente = e.Cliente.Idcliente;
            await Venda.UpdateInstance();
            Venda.IdclienteNavigation = e.Cliente;
            ReloadVenda();
        }
    }
}
