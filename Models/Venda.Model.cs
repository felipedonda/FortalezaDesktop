using FortalezaDesktop.Views;
using FortalezaDesktop.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.Models
{
    public partial class Venda : Model<Venda>
    {
        public decimal ValorTotal { get; set; }

        [JsonIgnore]
        public decimal ValorLiquido { get
            {
                return ValorTotal + Acrescimo - Desconto;
            }
        }

        [JsonIgnore]
        public decimal Troco
        {
            get
            {
                if(ValorPago > ValorLiquido)
                {
                    return ValorPago - ValorLiquido;
                }
                return 0;
            }
        }

        public override string Path { get { return "/vendas"; } }

        public override int? Id
        {
            get { return Idvenda; }
            set { Idvenda = value ?? default; }
        }

        public async Task GerarCupom()
        {
            TemplateCupomFiscal cupomFiscal = new TemplateCupomFiscal();
            cupomFiscal.GridInformacoesDelivery.Visibility = Visibility.Collapsed;
            cupomFiscal.GridResumoVenda.DataContext = this;
            cupomFiscal.GridNumeroPedido.Visibility = Visibility.Collapsed;

            if(IdclienteNavigation != null)
            {
                cupomFiscal.GridReciboCliente.DataContext = IdclienteNavigation;
            }
            else
            {
                cupomFiscal.GridReciboCliente.Visibility = Visibility.Collapsed;
            }

            InformacoesEmpresa informacoesEmpresa = await new InformacoesEmpresa().FindById(1);
            cupomFiscal.gridHeader.DataContext = informacoesEmpresa;

            cupomFiscal.TextHoraRecibo.Text = HoraEntrada.ToString("dd/MM/yy");
            cupomFiscal.TextHoraRecibo.Text += " " + HoraEntrada.ToString("hh:mm");
            cupomFiscal.mainGrid.ItemsSource = ItemVenda;

            RelatoriosVizualizador vizualizador = new RelatoriosVizualizador();
            vizualizador.LoadChildPage(cupomFiscal);
            vizualizador.Show();
        }

        public async Task<int> GetVendaAberta()
        {
            try
            {
                return await ServerEntry<int>.Get(Path + "/actions/aberta");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> HasVendaAberta()
        {
            try
            {
                return await ServerEntry<bool>.Get(Path + "/actions/hasaberta");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> FecharVenda()
        {
            try
            {
                await ServerEntry<Venda>.Get(Path + "/" + Id + "/actions/fechar");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> RecontarItems()
        {
            try
            {
                await ServerEntry<Venda>.Get(Path + "/" + Id + "/actions/recontaritems");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> SaveItemVenda(ItemVenda itemVenda)
        {
            try
            {
                await ServerEntry<Venda>.Post(Path + "/" + Id + "/itemvendas", itemVenda);
                var result = await ReloadInstance(new Dictionary<string, string>
                {
                    {"itemvendas","true"}
                });
                ItemVenda = result.ItemVenda;
                ValorTotal = result.ValorTotal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public async Task<bool> SavePagamento(Pagamento pagamento)
        {
            try
            {
                await ServerEntry<Venda>.Post(Path + "/" + Id + "/pagamentos", pagamento);
                var result = await ReloadInstance(new Dictionary<string, string>
                {
                    {"pagamentos","true"}
                });
                Pagamento = result.Pagamento;
                ValorPago = result.ValorPago;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
