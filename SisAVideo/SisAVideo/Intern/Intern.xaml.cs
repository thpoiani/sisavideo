using System;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;

namespace SisAVideo.Intern
{
    public partial class Intern : MetroWindow
    {
        private LoginRole usuario;
        private DatabaseConnection connection;
        private Videoconference videoconferencia;
        private Employer estagiario;
        private Count contagem;
        private Evaluation avaliacao;
        private URLScreen urlscreen;
        private Appointment apontamento;
        private Common.About sobre;

        public Intern()
        {
            InitializeComponent();

            usuario = new LoginRole();

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql_select = "SELECT f.nome FROM rh.funcionario f INNER JOIN rh.login l "
                                  + "ON f.id = l.id WHERE l.username = '" + usuario.Username + "'; ";

                lblNome.Content = connection.ReturnString(sql_select);
            }
            catch
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void videoconferenciaTile_Click(object sender, RoutedEventArgs e)
        {
            videoconferencia = new Videoconference();
            videoconferencia.ShowDialog();
        }

        private void estagiarioTile_Click(object sender, RoutedEventArgs e)
        {
            estagiario = new Employer();
            estagiario.ShowDialog();
        }

        private void avaliacaoTile_Click(object sender, RoutedEventArgs e)
        {
            avaliacao = new Evaluation();
            avaliacao.ShowDialog();
        }
        
        private void apontamentoTile_Click(object sender, RoutedEventArgs e)
        {
            apontamento = new Appointment();
            apontamento.ShowDialog();
        }

        private void streamingTile_Click(object sender, RoutedEventArgs e)
        {
            urlscreen = new URLScreen();
            urlscreen.ShowDialog();
        }

        private void contagemTile_Click(object sender, RoutedEventArgs e)
        {
            contagem = new Count();
            contagem.ShowDialog();
        }

        private void sobreTile_Click(object sender, RoutedEventArgs e)
        {
            sobre = new Common.About();
            sobre.ShowDialog();
        }

        private void siteTile_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink source = new Hyperlink();
            source.NavigateUri = new Uri("http://www.escolasdegoverno.sp.gov.br/");
            System.Diagnostics.Process.Start(source.NavigateUri.ToString());
        }

        public void erro()
        {
           Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                                           + "Entre em contato com o desenvolvedor", "Erro",
                                             MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
