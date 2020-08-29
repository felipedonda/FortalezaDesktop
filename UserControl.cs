using System;
using System.Collections.Generic;
using System.Text;
using FortalezaDesktop.Models;

namespace FortalezaDesktop
{
    public class UserControl
    {
        public static Usuario UsuarioLogado { get; set; }
        
        public static bool Loggar (string usuario, string senha)
        {
            //UsuarioLogado = Usuario.Loggar(usuario, senha);
            UsuarioLogado = new Usuario { Idusuario = 1, Nome = "Administrador" };
            return (UsuarioLogado != null);
        }
    }
}
