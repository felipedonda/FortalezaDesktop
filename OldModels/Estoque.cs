﻿using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class Estoque
    {
        public Estoque()
        {
            EstoqueHasFornecedor = new HashSet<EstoqueHasFornecedor>();
            EstoqueHasVenda = new HashSet<EstoqueHasVenda>();
            ItemHasEstoque = new HashSet<ItemHasEstoque>();
        }

        public int Idestoque { get; set; }
        public DateTime HoraEntrada { get; set; }
        public byte Saida { get; set; }
        public byte OrigemVenda { get; set; }
        public byte Disponivel { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal? Custo { get; set; }
        public string NotaFiscal { get; set; }
        public string Observacao { get; set; }

        public virtual ICollection<EstoqueHasFornecedor> EstoqueHasFornecedor { get; set; }
        public virtual ICollection<EstoqueHasVenda> EstoqueHasVenda { get; set; }
        public virtual ICollection<ItemHasEstoque> ItemHasEstoque { get; set; }
    }
}
