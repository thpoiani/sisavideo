using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;

namespace SisAVideo.Intern
{

    public partial class Count : MetroWindow
    {
        private LoginRole usuario;
        private DatabaseConnection connection;
        int contador_erros;

        public Count()
        {
            InitializeComponent();

            string retorno = null;

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                string sql = "SELECT titulo FROM vc.contagem_titulo ('" +usuario.Username+ "');";
                retorno = connection.ReturnString(sql);
            }
            catch
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            if (string.IsNullOrEmpty(retorno))
            {
                informationLabel.Content = "Seu polo não está ativado em nenhuma videoconferência agora.";
            }
            else
            {
                informationLabel.Content = retorno;
                numberBox.Visibility = Visibility.Visible;
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (numberBox.Visibility == Visibility.Hidden)
                this.Close();

            contador_erros = 0;
            Contagem contagem = new Contagem();

            try
            {
                contagem.Numero = Int32.Parse(numberBox.Text);
                numberBox.BorderBrush = Brushes.Black;
            }
            catch
            {
                numberBox.BorderBrush = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string check = "SELECT vc.contagem_check ('" + usuario.Username + "');";

                if (connection.Return(check) == 1) // 1 = is not null
                {
                    if (Xceed.Wpf.Toolkit.MessageBox.Show("Você já realizou a contagem, deseja refazer?", "Contagem", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        numberBox.Text = "";
                        return;
                    }
                }

                string sql = "EXEC vc.contagem_insert "
                           + "'" + usuario.Username
                           + "', " + contagem.Numero
                           + ";";

                connection.SQL(sql);
            }
            catch
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();

            Xceed.Wpf.Toolkit.MessageBox.Show("Contagem atribuída à Videoconferência.", "Contagem", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     
        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                                            + "Entre em contato com o desenvolvedor", "Erro",
                                              MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
