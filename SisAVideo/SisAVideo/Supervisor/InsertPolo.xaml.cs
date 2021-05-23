using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;

namespace SisAVideo.Supervisor
{
    public partial class InsertPolo : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        int contador_erros;
        private List<string> polos;

        public InsertPolo()
        {
            InitializeComponent();
            usuario = new LoginRole();
        }

        private void cadastrarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Polo polo = new Polo();

            contador_erros = 0;

            try
            {
                polo.Nome = nomeBox.Text;
                lblNome.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNome.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Endereco = enderecoBox.Text;
                lblEndereco.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblEndereco.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Cidade = cidadeBox.Text;
                lblCidade.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCidade.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Capacidade = Int32.Parse(capacidadeBox.Text);
                lblCapacidade.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCapacidade.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Horario_abertura = aberturaBox.Text;
                lblAbertura.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblAbertura.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Horario_fechamento = fechamentoBox.Text;
                lblFechamento.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblFechamento.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Email = emailBox.Text;
                lblEmail.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblEmail.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Senha = senhaBox.Text;
                lblSenha.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSenha.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                polo.Celular = celularBox.Text;
                lblCelular.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCelular.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                string sql_insert = "EXECUTE vc.cadastrar_polo "
                                  + "'"    + polo.Nome
                                  + "', '" + polo.Endereco
                                  + "', '" + polo.Cidade
                                  + "',  " + polo.Capacidade
                                  + ",  '" + polo.Horario_abertura
                                  + "', '" + polo.Horario_fechamento
                                  + "', '" + polo.Email
                                  + "', '" + polo.Senha
                                  + "', '" + polo.Celular
                                  + "' ;";

                connection.SQL(sql_insert);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();
            Xceed.Wpf.Toolkit.MessageBox.Show("Polo cadastrado.", "Polo", MessageBoxButton.OK, MessageBoxImage.Information);

            nomeBox.Text = "";
            enderecoBox.Text = "";
            cidadeBox.Text = "";
            capacidadeBox.Text = "";
            aberturaBox.Text = "";
            fechamentoBox.Text = "";
            emailBox.Text = "";
            senhaBox.Text = "";
            celularBox.Text = "";

            tabVisualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
        }





        private void tabAtualizar_Enable()
        {
            enderecoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(enderecoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverEndereco, 1);

            cidadeBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cidadeBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCidade, 1);

            capacidadeBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(capacidadeBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCapacidade, 1);

            aberturaBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(aberturaBoxAtualizar, 0);
            Panel.SetZIndex(lblOverAbertura, 1);
            
            fechamentoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(fechamentoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverFechamento, 1);

            emailBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(emailBoxAtualizar, 0);
            Panel.SetZIndex(lblOverEmail, 1);

            senhaBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(senhaBoxAtualizar, 0);
            Panel.SetZIndex(lblOverSenha, 1);

            celularBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(celularBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCelular, 1);
        }


        private void lblOverEndereco_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            enderecoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(enderecoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverEndereco, 0);
        }

        private void lblOverCidade_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cidadeBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cidadeBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCidade, 0);
        }

        private void lblOverCapacidade_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            capacidadeBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(capacidadeBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCapacidade, 0);
        }

        private void lblOverAbertura_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            aberturaBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(aberturaBoxAtualizar, 1);
            Panel.SetZIndex(lblOverAbertura, 0);
        }

        private void lblOverFechamento_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            fechamentoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(fechamentoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverFechamento, 0);
        }

        private void lblOverEmail_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            emailBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(emailBoxAtualizar, 1);
            Panel.SetZIndex(lblOverEmail, 0);
        }

        private void lblOverSenha_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            senhaBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(senhaBoxAtualizar, 1);
            Panel.SetZIndex(lblOverSenha, 0);
        }

        private void lblOverCelular_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            celularBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(celularBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCelular, 0);
        }


        private void enderecoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (enderecoBoxAtualizar.IsEnabled == true)
            {
                enderecoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(enderecoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverEndereco, 1);
            }
        }

        private void cidadeBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cidadeBoxAtualizar.IsEnabled == true)
            {
                cidadeBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(cidadeBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCidade, 1);
            }
        }

        private void capacidadeBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (capacidadeBoxAtualizar.IsEnabled == true)
            {
                capacidadeBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(capacidadeBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCapacidade, 1);
            }
        }

        private void aberturaBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (aberturaBoxAtualizar.IsEnabled == true)
            {
                aberturaBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(aberturaBoxAtualizar, 0);
                Panel.SetZIndex(lblOverAbertura, 1);
            }
        }

        private void fechamentoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (fechamentoBoxAtualizar.IsEnabled == true)
            {
                fechamentoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(fechamentoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverFechamento, 1);
            }
        }

        private void emailBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (emailBoxAtualizar.IsEnabled == true)
            {
                emailBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(emailBoxAtualizar, 0);
                Panel.SetZIndex(lblOverEmail, 1);
            }
        }

        private void senhaBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (senhaBoxAtualizar.IsEnabled == true)
            {
                senhaBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(senhaBoxAtualizar, 0);
                Panel.SetZIndex(lblOverSenha, 1);
            }
        }

        private void celularBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lblCelularAtualizar.IsEnabled == true)
            {
                lblCelularAtualizar.IsEnabled = false;
                Panel.SetZIndex(lblCelularAtualizar, 0);
                Panel.SetZIndex(lblOverCelular, 1);
            }
        }



        private void tabAtualizar_Loaded(object sender, RoutedEventArgs e)
        {
            tabAtualizar_Enable();

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT nome FROM vc.polo ORDER BY nome ASC");

                polos = new List<string>();

                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                    polos.Add(linha["nome"].ToString());

                nomeBoxAtualizar.ItemsSource = polos;
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();
        }

        private void nomeBoxAtualizar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tabAtualizar_Enable();

            try
            {
                string polo = nomeBoxAtualizar.SelectedValue.ToString();
                
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("EXEC vc.visualizar_polo_especifico '" + polo + "'; ");

                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                {
                    enderecoBoxAtualizar.Text = linha["endereco"].ToString();
                    cidadeBoxAtualizar.Text = linha["cidade"].ToString();
                    capacidadeBoxAtualizar.Text = linha["capacidade"].ToString();
                    aberturaBoxAtualizar.Text = linha["horario_abertura"].ToString();
                    fechamentoBoxAtualizar.Text = linha["horario_fechamento"].ToString();
                    emailBoxAtualizar.Text = linha["email"].ToString();
                    senhaBoxAtualizar.Text = linha["senha"].ToString();
                    celularBoxAtualizar.Text = linha["celular"].ToString();
                }
                lblNomeAtualizar.Foreground = Brushes.Black;
            }
            catch
            {
                lblNomeAtualizar.Foreground = Brushes.Red;
            }
        }

        private void atualizarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Polo polo = new Polo();

            contador_erros = 0;
            int contador_mudancas = 0;
            string sql_update = "BEGIN TRANSACTION; ";

            if (enderecoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Endereco = enderecoBoxAtualizar.Text;
                    lblEnderecoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "endereco = '" + polo.Endereco + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() +"'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblEnderecoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cidadeBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Cidade = cidadeBoxAtualizar.Text;
                    lblCidadeAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "cidade = '" + polo.Cidade + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCidadeAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (capacidadeBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Capacidade = Int32.Parse(capacidadeBoxAtualizar.Text);
                    lblCapacidadeAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "capacidade = " + polo.Capacidade + " "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCapacidadeAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (aberturaBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Horario_abertura = aberturaBoxAtualizar.Text;
                    lblAberturaAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "horario_abertura = '" + polo.Horario_abertura + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblAberturaAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (fechamentoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Horario_fechamento = fechamentoBoxAtualizar.Text;
                    lblFechamentoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "horario_fechamento = '" + polo.Horario_fechamento + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblFechamentoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (emailBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Email = emailBoxAtualizar.Text;
                    lblEmailAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "email = '" + polo.Email + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblEmailAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (senhaBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Senha = senhaBoxAtualizar.Text;
                    lblSenhaAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "senha = '" + polo.Senha + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblSenhaAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (celularBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    polo.Celular = celularBoxAtualizar.Text;
                    lblCelularAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.polo SET "
                                + "celular = '" + polo.Celular + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCelularAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            sql_update += "COMMIT; ";

            if (contador_erros != 0)
            {
                sql_update = null;
                return;
            }

            if (contador_mudancas == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Nenhuma atualização registrada.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                connection.SQL(sql_update);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();
            Xceed.Wpf.Toolkit.MessageBox.Show("Polo atualizado.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);

            tabVisualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
        }



        private void tabRemover_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT nome FROM vc.polo ORDER BY nome ASC");

                polos = new List<string>();

                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                    polos.Add(linha["nome"].ToString());

                nomeBoxRemover.ItemsSource = polos;
            }
            catch (SqlException)
            {
               erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }



        private void removerSendButton_Click(object sender, RoutedEventArgs e)
        {
          try
            {
                string polo = nomeBoxRemover.SelectedValue.ToString();

                if (Xceed.Wpf.Toolkit.MessageBox.Show("Deseja remover o polo \n" + polo + " ?", "Remoção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();

                    string sql_delete = "DELETE FROM vc.polo WHERE nome = '" + polo + "'; ";

                    connection.SQL(sql_delete);

                    Xceed.Wpf.Toolkit.MessageBox.Show("Polo removido com sucesso.");
                }
            }
            catch
            {
            }

            connection.Close();

            tabVisualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
        }

        private void tabVisualizar_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM vc.visualizar_polo";

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



        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                          + "Entre em contato com o desenvolvedor", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
