using FortalezaDesktop.Models;
using FortalezaDesktop.Controllers;
using FortalezaDesktop.Views;
using FortalezaDesktop.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace FortalezaDesktop.Controllers
{
    public class RelatoriosController
    {

        public static void ImprimirCupom(Venda venda)
        {
            if (UserPreferences.Preferences.ImpressoraCupom.Habilitada)
            {
                if (UserPreferences.Preferences.ImpressoraCupom.SempreImprimir)
                {
                    GerarCupom(venda);
                }
                else
                {
                    var result = MessageBox.Show("Deseja imprimir o cupom?", "Concluir venda", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        GerarCupom(venda);
                    }
                }
            }
        }

        public static async void GerarCupomPedido(Pedido pedido)
        {
            TemplateCupomFiscal cupomFiscal = new TemplateCupomFiscal();
            cupomFiscal.GridInformacoesDelivery.DataContext = pedido;
            cupomFiscal.GridResumoVenda.DataContext = pedido.IdvendaNavigation;
            cupomFiscal.TextNumeroPedido.Text += pedido.NumeroPedido;
            cupomFiscal.GridReciboCliente.DataContext = pedido.IdvendaNavigation.IdclienteNavigation;
            cupomFiscal.TextHoraRecibo.Text = pedido.IdvendaNavigation.HoraEntrada.ToString("dd/MM/yy");
            cupomFiscal.TextHoraRecibo.Text += " " + pedido.IdvendaNavigation.HoraEntrada.ToString("hh:mm");
            cupomFiscal.mainGrid.ItemsSource = pedido.IdvendaNavigation.ItemVenda;

            InformacoesEmpresa informacoesEmpresa = await InformacoesEmpresaController.GetInformacoesEmpresa();
            cupomFiscal.gridHeader.DataContext = informacoesEmpresa;

            if (UserPreferences.Preferences.ImpressoraCupom.Visualizar)
            {
                RelatoriosVizualizador vizualizador = new RelatoriosVizualizador(RelatoriosVizualizador.TipoEmpressaoEnum.Cupom);
                vizualizador.LoadChildPage(cupomFiscal);
                vizualizador.ShowDialog();
            }
            else
            {
                PrintersController.PrintCupom(cupomFiscal);
            }
        }

        public static async void GerarCupom(Venda venda)
        {
            TemplateCupomFiscal cupomFiscal = new TemplateCupomFiscal();
            cupomFiscal.GridInformacoesDelivery.Visibility = Visibility.Collapsed;
            cupomFiscal.GridResumoVenda.DataContext = venda;
            cupomFiscal.GridNumeroPedido.Visibility = Visibility.Collapsed;
            cupomFiscal.Width = UserPreferences.Preferences.ImpressoraCupom.Tamanho;

            if (venda.IdclienteNavigation != null)
            {
                cupomFiscal.GridInfoCliente.DataContext = venda.IdclienteNavigation;
            }
            else
            {
                cupomFiscal.GridInfoCliente.Visibility = Visibility.Collapsed;
            }

            InformacoesEmpresa informacoesEmpresa = await InformacoesEmpresaController.GetInformacoesEmpresa();
            cupomFiscal.gridHeader.DataContext = informacoesEmpresa;

            cupomFiscal.TextTituloRecibo.Text += " " + venda.Idvenda;

            cupomFiscal.TextHoraRecibo.Text = venda.HoraEntrada.ToString("dd/MM/yy");
            cupomFiscal.TextHoraRecibo.Text += " " + venda.HoraEntrada.ToString("hh:mm");
            cupomFiscal.mainGrid.ItemsSource = venda.ItemVenda;
            
            if(UserPreferences.Preferences.ImpressoraCupom.Visualizar)
            {
                RelatoriosVizualizador vizualizador = new RelatoriosVizualizador(RelatoriosVizualizador.TipoEmpressaoEnum.Cupom);
                vizualizador.LoadChildPage(cupomFiscal);
                vizualizador.ShowDialog();
            }
            else
            {
                PrintersController.PrintCupom(cupomFiscal);
            }
        }
    }
}
