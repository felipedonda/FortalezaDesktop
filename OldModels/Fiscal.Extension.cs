using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    public partial class Fiscal
    {
        public class ListItem
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }

        public static List<ListItem> ListaCfop = new List<ListItem>
        {
            new ListItem { Value = 5101, Display = "5101 - Venda de produção do estabelecimento" },
            new ListItem { Value = 5102, Display = "5102 - Venda de mercadoria adquirida ou recebida de terceiros" }
        };

        public static List<ListItem> ListaOrigem = new List<ListItem>
        {
            new ListItem { Value = 0, Display = "0 - Nacional" },
            new ListItem { Value = 1, Display = "1 - Estrangeira - Importação direta" },
            new ListItem { Value = 2, Display = "2 - Estrangeira - Adquirida no mercado interno" },
            new ListItem { Value = 3, Display = "3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%" },
            new ListItem { Value = 4, Display = "4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam o Decreto-Lei nº 288/67..." },
            new ListItem { Value = 5, Display = "5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%" },
            new ListItem { Value = 6, Display = "6 - Estrangeira – Importação direta, sem similar nacional, constante em lista de Resolução CAMEX e gás natural" },
            new ListItem { Value = 7, Display = "7 - Estrangeira – Adquirida no mercado interno, sem similar nacional, constante em lista de Resolução CAMEX e gás natural" },
            new ListItem { Value = 8, Display = "8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%" },
        };

        public static List<ListItem> ListaCstIcms = new List<ListItem>
        {
            new ListItem { Value = 0, Display = "00 - Tributada integralmente" },
            new ListItem { Value = 10, Display = "10 - Tributada e com cobrança do ICMS por substituição tributária" },
            new ListItem { Value = 20, Display = "20 - Com redução da BC" },
            new ListItem { Value = 30, Display = "30 - Isenta / não tributada e com cobrança do ICMS por substituição tributária" },
            new ListItem { Value = 40, Display = "40 - Isenta" },
            new ListItem { Value = 41, Display = "41 - Não tributada" },
            new ListItem { Value = 50, Display = "50 - Com suspensão" },
            new ListItem { Value = 51, Display = "51 - Com diferimento" },
            new ListItem { Value = 60, Display = "60 - ICMS cobrado anteriormente por substituição tributária" },
            new ListItem { Value = 70, Display = "70 - Com redução de base de cálculo e cobrança do ICMS por substituição tributária" },
            new ListItem { Value = 90, Display = "90 - Outras" },
        };
    }
}
