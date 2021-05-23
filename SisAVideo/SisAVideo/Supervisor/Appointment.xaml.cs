using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MahApps.Metro.Controls;

namespace SisAVideo.Supervisor
{
    public partial class Appointment : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private List<string> estagiarios;
        private string nome;
        private string dia;

        public Appointment()
        {
            InitializeComponent();
        }

        private void tabApontamento_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT * FROM rh.apontamento_visualizar");
                gridApontamentos.DataContext = connection.View("apontamento");

                connection.SQL("SELECT f.nome as nome FROM rh.funcionario f INNER JOIN rh.estagiario e ON f.id = e.id ORDER BY nome ASC");

                estagiarios = new List<string>();
                estagiarios.Add("");

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                    estagiarios.Add(linha["nome"].ToString());

                nomeBox.ItemsSource = estagiarios;
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }


        private void nomeBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            nome = nomeBox.SelectedValue.ToString();
            string sql = null;

            if ((string.IsNullOrEmpty(nome)) && (string.IsNullOrEmpty(dia)))
                sql = "SELECT * FROM rh.apontamento_visualizar";
            else
                if ((!string.IsNullOrEmpty(nome)) && (string.IsNullOrEmpty(dia)))
                    sql = "SELECT * FROM rh.apontamento_visualizar WHERE nome = '" + nome + "'";
                else
                    sql = "SELECT * FROM rh.apontamento_visualizar WHERE nome = '" + nome + "' AND dia = '" + dia + "'";

            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL(sql);
                gridApontamentos.DataContext = connection.View("apontamento");
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void dataBox_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            dia = dataBox.SelectedDate.ToString().Substring(0,10);
            
            string sql = null;

            if ((string.IsNullOrEmpty(dia)) && (string.IsNullOrEmpty(nome)))
                sql = "SELECT * FROM rh.apontamento_visualizar";
            else
                if ((!string.IsNullOrEmpty(dia)) && (string.IsNullOrEmpty(nome)))
                    sql = "SELECT * FROM rh.apontamento_visualizar WHERE dia = '" + dia + "'";
                else
                    sql = "SELECT * FROM rh.apontamento_visualizar WHERE dia = '" + dia + "' AND nome = '" + nome + "'";

            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL(sql);
                gridApontamentos.DataContext = connection.View("apontamento");
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
            
        }

        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                          + "Entre em contato com o desenvolvedor", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
