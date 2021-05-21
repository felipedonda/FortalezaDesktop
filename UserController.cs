using System;
using System.Collections.Generic;
using System.Text;
using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;

namespace FortalezaDesktop
{
    public class UserController
    {
        public static Usuario UsuarioLogado { get; set; }
        
        public static bool Loggar (string usuario, string senha)
        {
            //UsuarioLogado = Usuario.Loggar(usuario, senha);
            UsuarioLogado = new Usuario { Idusuario = 1, Nome = "Administrador" };
            Logger.Log("Usuário '" + UsuarioLogado.Nome + "' logado.", Logger.LogType.Info);
            return (UsuarioLogado != null);
        }
    }
}
