using FortalezaDesktop.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace FortalezaDesktop.Models
{
    public class Item
    {
        public int? Iditem { get; set; }
        
        public string Descricao { get; set; }
        
        public string Unidade { get; set; }

        public string Imagem { get; set; }

        public decimal Valor { get; set; }

        public decimal EstoqueMinimo { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Visivel { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Disponivel { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Estoque { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool PermiteCombo { get; set; }

        public string Tipo { get; set; }

        [JsonIgnore]
        public Estoque EstoqueAtual { get; set; }

        [JsonIgnore]
        public List<Estoque> Estoques { get; set; }

        [JsonIgnore]
        public List<Estoque> EstoquesDisponiveis { get; set; }

        [JsonIgnore]
        public Pacote Pacote { get; set; }

        [JsonIgnore]
        public List<Grupo> Grupos { get; set; }

        public async Task SaveInstance()
        {
            Iditem = (await CreateItem(this)).Iditem;
            if(Grupos != null)
            {
                await AddGrupos(Iditem ?? default, Grupos);
            }
            if(Estoque & EstoqueAtual != null & Tipo == "Produto")
            {
                await CreateEstoque(EstoqueAtual.QuantidadeDisponivel, EstoqueAtual.Custo);
            }
            if(Tipo == "Pacote")
            {
                if(Pacote.IditemPacote == null)
                {
                    Pacote.IditemPacote = Pacote.ItemPacote.Iditem;
                }
                await CreatePacote(Iditem ?? default, Pacote);
            }
        }

        public async Task CreateEstoque(decimal quantidade)
        {
            Estoque estoque = new Estoque
            {
                HoraEntrada = DateTime.UtcNow,
                OrigemVenda = false
            };

            if (quantidade < 0)
            {
                estoque.Saida = true;
                estoque.Quantidade = -quantidade;
            }
            else
            {
                estoque.Saida = false;
                estoque.Quantidade = quantidade;
            }
            
            await CreateEstoque(estoque);
        }

        public async Task<List<Estoque>> CreateEstoque(decimal quantidade, int vendaIdvenda)
        {
            Estoque estoque = new Estoque
            {
                HoraEntrada = DateTime.UtcNow,
                Quantidade = quantidade,
                Saida = true,
                OrigemVenda = true,
                VendaIdvenda = vendaIdvenda
            };
            return await CreateEstoque(estoque);
        }

        public async Task CreateEstoque(decimal quantidade, decimal custo)
        {
            Estoque estoque = new Estoque
            {
                HoraEntrada = DateTime.UtcNow,
                Quantidade = quantidade,
                Custo = custo,
                Saida = false,
                OrigemVenda = false
            };
            await CreateEstoque(estoque);
        }

        public async Task<List<Estoque>> CreateEstoque(Estoque estoque)
        {
            switch(Tipo)
            {
                case "Produto":
                    estoque.ItemIditem = Iditem;
                    if (estoque.Saida)
                    {
                        estoque.Disponivel = false;
                        await LoadEstoqueAtual();
                        if (estoque.Quantidade > EstoqueAtual.QuantidadeDisponivel)
                        {
                            throw new Exception("Estoque insuficiente.");
                        }
                        await Models.Estoque.CreateEstoque(estoque);
                        return await AtualizarEstoquesDisponiveis(EstoquesDisponiveis.OrderBy(e => e.HoraEntrada).ToList(), estoque.Quantidade);
                    }
                    else
                    {
                        estoque.Disponivel = true;
                        estoque.QuantidadeDisponivel = estoque.Quantidade;
                        await Models.Estoque.CreateEstoque(estoque);
                        return null;
                    }
                case "Pacote":
                    await LoadPacote();
                    estoque.Custo /= Pacote.Quantidade;
                    estoque.Quantidade *= Pacote.Quantidade;
                    return await Pacote.ItemPacote.CreateEstoque(estoque);
                default:
                    throw new Exception("Tipo de item inválido.");
            }
        }

        public static async Task<List<Estoque>> AtualizarEstoquesDisponiveis(List<Estoque> estoquesDisponiveis, decimal quantidade)
        {
            List<Estoque> custos = new List<Estoque>();
            Estoque estoque = estoquesDisponiveis[0];
            if (estoque.QuantidadeDisponivel > quantidade)
            {
                estoque.QuantidadeDisponivel -= quantidade;
                custos.Add(new Estoque { Custo = estoque.Custo, QuantidadeDisponivel = quantidade });
                await estoque.UpdateInstance();
                return custos;
            }
            else
            {
                decimal restante = quantidade - estoque.QuantidadeDisponivel;
                custos.Add(new Estoque { Custo = estoque.Custo, QuantidadeDisponivel = estoque.QuantidadeDisponivel });
                estoque.QuantidadeDisponivel = 0;
                estoque.Disponivel = false;
                await estoque.UpdateInstance();
                if (restante > 0)
                {
                    custos.AddRange(await AtualizarEstoquesDisponiveis(estoquesDisponiveis.GetRange(1, estoquesDisponiveis.Count - 1), restante));
                    return custos;
                }
                else
                {
                    return custos;
                }
            }
        }

        public async Task LoadEstoques()
        {
            Estoques = await GetEstoques(Iditem ?? default);
        }

        public async Task LoadPacote()
        {
            Pacote = await GetPacote(Iditem ?? default);
            Pacote.ItemPacote = await Item.GetItem(Pacote.IditemPacote ?? default);
        }

        public async Task LoadEstoquesDisponiveis()
        {
            if(Tipo == "Produto")
            {
                EstoquesDisponiveis = await GetEstoquesDisponiveis(Iditem ?? default);
            }
            else
            {
                throw new Exception("Tipo de item inválido para operação.");
            }
            
        }

        public async Task LoadEstoqueAtual()
        {
            switch(Tipo)
            {
                case "Produto":
                    await LoadEstoquesDisponiveis();
                    if (EstoquesDisponiveis != null)
                    {
                        if (EstoquesDisponiveis.Count > 0)
                        {
                            EstoqueAtual = new Estoque
                            {
                                QuantidadeDisponivel = EstoquesDisponiveis.Sum(e => e.QuantidadeDisponivel),
                                Custo = EstoquesDisponiveis.OrderBy(e => e.HoraEntrada).First().Custo
                            };
                        }
                        else
                        {
                            EstoqueAtual = new Estoque
                            {
                                QuantidadeDisponivel = 0,
                                Custo = 0
                            };
                        }
                    }
                    break;
                case "Pacote":
                    await LoadPacote();
                    await Pacote.ItemPacote.LoadEstoqueAtual();
                    if (Pacote.ItemPacote.EstoquesDisponiveis.Count > 0)
                    {
                        EstoqueAtual = new Estoque
                        {
                            QuantidadeDisponivel = Pacote.ItemPacote.EstoquesDisponiveis.Sum(e => e.QuantidadeDisponivel)/Pacote.Quantidade,
                            Custo = Pacote.ItemPacote.EstoquesDisponiveis.OrderBy(e => e.HoraEntrada).First().Custo*Pacote.Quantidade
                        };
                    }
                    else
                    {
                        EstoqueAtual = new Estoque
                        {
                            QuantidadeDisponivel = 0,
                            Custo = 0
                        };
                    }
                    break;
            }

        }

        public async Task<bool> ValidarEstoqueDisponivel(decimal quantidade)
        {
            await LoadEstoqueAtual();
            if(EstoqueAtual == null)
            {
                return false;
            }
            return EstoqueAtual.QuantidadeDisponivel >= quantidade;
        }

        public async Task LoadGrupos()
        {
            if (Iditem != null)
            {
                Grupos = await GetGrupos(Iditem ?? default);
            }
        }

        public async Task UpdateInstance()
        {
            if (Iditem != null)
            {
                await UpdateItem(Iditem ?? default, this);
                await UpdateGrupos(Iditem ?? default, Grupos);
                if (Tipo == "Pacote")
                {
                    await UpdatePacote(Iditem ?? default, Pacote);
                }
            }
        }

        public static async Task<List<Item>> GetItems()
        {
            return await Model<List<Item>>.Get("/item");
        }

        public static async Task<Item> GetItem(int id)
        {
            return await Model<Item>.Get("/item", id);
        }

        public static async Task<Item> CreateItem(Item item)
        {
            return await Model<Item>.Post("/item", item);
        }

        public static async Task<bool> UpdateItem(int id, Item item)
        {
            return await Model<Item>.Put("/item", id, item);
        }

        public static async Task<bool> DeleteItem(int id)
        {
            return await Model<Item>.Delete("/item", id);
        }

        public static async Task<Pacote> GetPacote(int iditem)
        {
            return await Model<Pacote>.Get("/item/" + iditem + "/pacote");
        }

        public static async Task<Pacote> CreatePacote(int iditem, Pacote pacote)
        {
            return await Model<Pacote>.Post("/item/" + iditem + "/pacote", pacote);
        }

        public static async Task<bool> UpdatePacote(int iditem, Pacote pacote)
        {
            return await Model<Pacote>.Put("/item/" + iditem + "/pacote", pacote);
        }

        public static async Task<List<Estoque>> GetEstoques(int iditem)
        {
            return await Model<List<Estoque>>.Get("/item/" + iditem + "/estoque");
        }

        public static async Task<List<Estoque>> GetEstoquesDisponiveis(int iditem)
        {
            return await Model<List<Estoque>>.Get("/item/" + iditem + "/estoque-disponivel");
        }

        public static async Task<Estoque> GetEstoqueAtual(int iditem)
        {
            return await Model<Estoque>.Get("/item/" + iditem + "/estoque-atual");
        }

        public static async Task<Estoque> PostEstoque(int iditem, Estoque estoque)
        {
            return await Model<Estoque>.Post("/item/" + iditem + "/estoque", estoque);
        }

        public static async Task<List<Grupo>> GetGrupos(int idItem)
        {
            return await Model<List<Grupo>>.Get("/item/" + idItem + "/grupo");
        }

        public static async Task AddGrupos(int idItem, List<Grupo> grupos)
        {
            await Model<List<Grupo>>.Post("/item/" + idItem + "/grupo", grupos);
        }

        public static async Task<bool> UpdateGrupos(int idItem, List<Grupo> grupos)
        {
            return await Model<List<Grupo>>.Put("/item/" + idItem + "/grupo", grupos);
        }
    }
}
