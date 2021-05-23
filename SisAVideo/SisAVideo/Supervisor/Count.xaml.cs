using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace SisAVideo.Supervisor
{
    public partial class Count : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private List<string> videos;

        public Count()
        {
            InitializeComponent();
        }

        private void tabContagem_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT * FROM vc.visualizar_videoconferencia ORDER BY codigo DESC");

                videos = new List<string>();

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    videos.Add("[" + linha["codigo"].ToString() + "] " + linha
["titulo"].ToString() + " (" + linha["data_ativacao"] + " " + linha["horario_inicio"] + ") ");

                videoBox.ItemsSource = videos;
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void videoBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            string sql = "EXEC vc.visualizar_contagem '" +videoBox.SelectedValue.ToString()+ "'";
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                connection.SQL(sql);
                gridPolos.DataContext = connection.View("polo");
            }
            catch (SqlException)
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
