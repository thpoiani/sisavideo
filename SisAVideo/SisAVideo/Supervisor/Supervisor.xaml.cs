using System;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;

namespace SisAVideo.Supervisor
{
    public partial class Supervisor : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private Videoconference videoconferencia;
        private InsertPolo polo;
        private Employer funcionario;
        private Intern estagiario;
        private Appointment apontamento;
        private Count contagem;
        private Common.About sobre;

        public Supervisor()
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
                //Application.Current.Shutdown();
            }
            connection.Close();
        }
        
        private void videoconferenciaTile_Click(object sender, RoutedEventArgs e)
        {
            videoconferencia = new Videoconference();
            videoconferencia.tabCadastrar.Focus();
            videoconferencia.ShowDialog();
        }

        private void poloTile_Click(object sender, RoutedEventArgs e)
        {
            polo = new InsertPolo();
            polo.tabCadastrar.Focus();
            polo.ShowDialog();
        }

        private void funcionarioTile_Click(object sender, RoutedEventArgs e)
        {
            funcionario = new Employer();
            funcionario.tabCadastrar.Focus();
            funcionario.ShowDialog();
        }

        private void estagiarioTile_Click(object sender, RoutedEventArgs e)
        {
            estagiario = new Intern();
            estagiario.tabCadastrar.Focus();
            estagiario.ShowDialog();
        }

        private void apontamentoTile_Click(object sender, RoutedEventArgs e)
        {
            apontamento = new Appointment();
            apontamento.ShowDialog();
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

        private void erro()
        {
            MessageBox.Show("Erro de acesso ao banco de dados.\n"
                          + "Entre em contato com o desenvolvedor", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
