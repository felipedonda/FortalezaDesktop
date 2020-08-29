using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for ConfiguracoesView.xaml
    /// </summary>
    public partial class Configuracoes : Window
    {
        public event EventHandler UpdateParent;

        public Configuracoes()
        {
            InitializeComponent();
            UserPreferences.Load();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            LoadConfiguracoes();
        }

        public void LoadConfiguracoes()
        {
            checkboxGrupoTodos.IsChecked = UserPreferences.GrupoTodos;
            checkboxModoCaixa.IsChecked = UserPreferences.ModoCaixa;
        }

        private void checkboxGrupoTodos_Checked(object sender, RoutedEventArgs e)
        {
            UserPreferences.GrupoTodos = checkboxGrupoTodos.IsChecked ?? false;
            UserPreferences.Save();
            UpdateParent?.Invoke(this, new EventArgs());
        }

        private void checkboxModoCaixa_Checked(object sender, RoutedEventArgs e)
        {
            UserPreferences.ModoCaixa = checkboxModoCaixa.IsChecked ?? false;
            UserPreferences.Save();
            UpdateParent?.Invoke(this, new EventArgs());
        }
    }
}
