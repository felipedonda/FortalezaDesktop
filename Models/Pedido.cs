using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class Pedido
    {
        public int Idvenda { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime? DataPrazo { get; set; }
        public DateTime? DataEntregue { get; set; }
        public byte? Entregue { get; set; }
        public int? Status { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime? DateRetorno { get; set; }
        public byte? Delivery { get; set; }
        public int? Identregador { get; set; }

        public virtual Entregador IdentregadorNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
    }
}
