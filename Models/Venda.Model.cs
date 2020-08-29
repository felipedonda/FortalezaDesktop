using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;
using FortalezaDesktop;

namespace FortalezaDesktop.Models
{
    public class Venda
    {
        public int? Idvenda { get; set; }
        public string Nome { get; set; }
        public DateTime HoraEntrada { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? CustoTotal { get; set; }
        public decimal? Alteracao { get; set; }
        public string Observacao { get; set; }
        public int Idresponsavel { get; set; }

        [JsonIgnore]
        public Usuario Responsavel {get;set;}

        [JsonConverter(typeof(BoolConverter))]
        public bool Aberta { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Paga { get; set; }

        [JsonIgnore]
        public List<ItemVenda> Items { get; set; }

        [JsonIgnore]
        public List<Pagamento> Pagamentos { get; set; }

        [JsonIgnore]
        public int? Idcaixa { get; set; }

        public Venda()
        {
        }

        public Venda(Caixa caixa)
        {
            Pagamentos = new List<Pagamento>();
            Items = new List<ItemVenda>();
            HoraEntrada = DateTime.UtcNow;
            if (!caixa.Aberto)
                throw new Exception("Caixa fechado.");
            Idcaixa = caixa.Idcaixa;
            ValorTotal = new decimal();
        }

        public async Task<Pagamento> GerarPagamento(decimal valor, FormaPagamento formaPagamento, int? BandeiraIdbandeira)
        {
            Pagamento pagamento = new Pagamento
            {
                Movimento = new Movimento
                {
                    CaixaIdcaixa = Idcaixa ?? default,
                    FormaPagamento = formaPagamento,
                    FormaPagamentoIdformaPagamento = formaPagamento.IdformaPagamento,
                    HoraEntrada = DateTime.UtcNow,
                    Responsavel = Idresponsavel,
                    Tipo = "C",
                    Valor = valor,
                    Descricao = "VENDA - CONSUMIDOR NÃO IDENTIFICADO",
                }
            };

            if(BandeiraIdbandeira != null)
            {
                pagamento.Movimento.BandeiraIdbandeira = BandeiraIdbandeira ?? default;
            }

            return pagamento;
        }

        public async Task AddItemVenda(Item item, decimal quantidade)
        {
            if (Idcaixa == null)
            {
                throw new Exception("Venda sem caixa atribuida.");
            }
            else
            {
                ItemVenda itemVenda = new ItemVenda
                {
                    ItemIditem = item.Iditem ?? default,
                    Item = item,
                    Valor = item.Valor,
                    Quantidade = quantidade,
                    ValorTotal = item.Valor * quantidade
                };
                ValorTotal += itemVenda.ValorTotal;
                Items.Add(itemVenda);
            }
        }

        public async Task SaveInstance()
        {
            Idvenda = null;
            Idvenda = (await CreateVenda(this)).Idvenda;

            if (Items.Count < 0)
                throw new Exception("Venda vazia.");

            if (Pagamentos.Count < 0)
                throw new Exception("Venda sem pagamentos.");

            foreach (ItemVenda itemVenda in Items)
            {
                itemVenda.VendaIdvenda = Idvenda ?? default;
                if(itemVenda.Item.Estoque)
                {

                    List<Estoque> custos = await itemVenda.Item.CreateEstoque(itemVenda.Quantidade, Idvenda ?? default);
                    foreach(var c in custos)
                    {
                        ItemVenda iv = itemVenda;
                        iv.Quantidade = c.QuantidadeDisponivel;
                        iv.Custo = c.Custo;
                        if (itemVenda.Item.Tipo == "Pacote")
                        {
                            iv.Quantidade /= itemVenda.Item.Pacote.Quantidade;
                            iv.Custo *= itemVenda.Item.Pacote.Quantidade;
                        }
                        await CreateItemVenda(Idvenda ?? default, iv);
                    }
                }
                else
                {
                    itemVenda.IditemVenda = (await CreateItemVenda(Idvenda ?? default, itemVenda)).IditemVenda;
                }
            }

            foreach (Pagamento pagamento in Pagamentos)
            {
                pagamento.VendaIdvenda = Idvenda ?? default;
                pagamento.Movimento.Idmovimento = null;
                pagamento.Movimento = await Caixa.CreateMovimento(Idcaixa ?? default, pagamento.Movimento);
                pagamento.MovimentoIdmovimento = pagamento.Movimento.Idmovimento;
                pagamento.IdPagamento = (await CreatePagamento(Idvenda ?? default, pagamento)).IdPagamento;
            }
        }

        public static async Task<List<Venda>> GetVendas()
        {
            return await Model<List<Venda>>.Get("/venda");
        }

        public static async Task<Venda> GetVenda(int id)
        {
            return await Model<Venda>.Get("/venda", id);
        }

        public static async Task<Venda> CreateVenda(Venda venda)
        {
            return await Model<Venda>.Post("/venda", venda);
        }

        public static async Task<bool> UpdateVenda(int id, Venda venda)
        {
            return await Model<Venda>.Put("/venda", id, venda);
        }

        public static async Task<bool> DeleteVenda(int id)
        {
            return await Model<Venda>.Delete("/venda", id);
        }

        public static async Task<List<ItemVenda>> GetItemVendas(int idvenda)
        {
            return await Model<List<ItemVenda>>.Get("/venda/" + idvenda + "/itemvenda");
        }

        public static async Task<ItemVenda> GetItemVenda(int idvenda, int iditemVenda)
        {
            return await Model<ItemVenda>.Get("/venda/" + idvenda + "/itemvenda", iditemVenda);
        }

        public static async Task<ItemVenda> CreateItemVenda(int idvenda, ItemVenda itemVenda)
        {
            return await Model<ItemVenda>.Post("/venda/" + idvenda + "/itemvenda", itemVenda);
        }

        public static async Task<Pagamento> CreatePagamento(int idvenda, Pagamento pagamento)
        {
            return await Model<Pagamento>.Post("/venda/" + idvenda + "/pagamento", pagamento);
        }

    }
}
