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
    public partial class Intern : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private List<string> estagiarios;
        private List<string> polos;
        public string estagiario_novo;
        int contador_erros;

        public Intern()
        {
            InitializeComponent();
        }

        private void tabCadastrar_Loaded(object sender, RoutedEventArgs e)
        {
            poloBox.IsEnabled = false;
            instituicaoBox.IsEnabled = false;
            cursoBox.IsEnabled = false;
            datainiPicker.IsEnabled = false;
            datafimPicker.IsEnabled = false;
            datacontratoPicker.IsEnabled = false;
            entradaBox.IsEnabled = false;
            saidaBox.IsEnabled = false;


            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT * FROM rh.estagiario_nao_cadastrado; ");
                estagiarios = new List<string>();

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                    estagiarios.Add(linha["nome"].ToString());
                nomeBox.ItemsSource = estagiarios;

                connection.SQL("SELECT nome FROM vc.polo ORDER BY nome ASC; ");
                polos = new List<string>();

                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                    polos.Add(linha["nome"].ToString());

                poloBox.ItemsSource = polos;
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            if (!string.IsNullOrEmpty(estagiario_novo))
            {
                nomeBox.Text = estagiario_novo;
            }
        }

        private void nomeBox_SelectionChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            poloBox.IsEnabled = true;
            instituicaoBox.IsEnabled = true;
            cursoBox.IsEnabled = true;
            datainiPicker.IsEnabled = true;
            datafimPicker.IsEnabled = true;
            datacontratoPicker.IsEnabled = true;
            entradaBox.IsEnabled = true;
            saidaBox.IsEnabled = true;
        }

        private void cadastrarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Estagiario estagiario = new Estagiario();
            usuario = new LoginRole();

            contador_erros = 0;

            try
            {
                estagiario.Nome_funcionario = nomeBox.SelectedValue.ToString();
                lblNome.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNome.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Nome_polo = poloBox.SelectedValue.ToString();
                lblPolo.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblPolo.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Instituicao = instituicaoBox.Text;
                lblInstituicao.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblInstituicao.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Curso = cursoBox.Text;
                lblCurso.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCurso.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Inicio_curso = datainiPicker.Text;
                lblDataIni.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblDataIni.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Fim_curso = datafimPicker.Text;
                lblDataFim.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblDataFim.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Data_contrato = datacontratoPicker.Text;
                lblDataContrato.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblDataContrato.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Horario_entrada = entradaBox.Text;
                lblEntrada.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblEntrada.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                estagiario.Horario_saida = saidaBox.Text;
                lblSaida.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSaida.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                string sql_insert = "EXEC rh.cadastrar_estagiario "
                                  + "'" + estagiario.Nome_funcionario
                                  + "', '" + estagiario.Nome_polo
                                  + "', '" + estagiario.Instituicao
                                  + "', '" + estagiario.Curso
                                  + "', '" + estagiario.Inicio_curso
                                  + "', '" + estagiario.Fim_curso
                                  + "', '" + estagiario.Data_contrato
                                  + "', '" + estagiario.Horario_entrada
                                  + "', '" + estagiario.Horario_saida + "';";

                connection.SQL(sql_insert);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();
            Xceed.Wpf.Toolkit.MessageBox.Show("Estagiário cadastrado.", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);

            nomeBox.Text = "";
            poloBox.Text = "";
            instituicaoBox.Text = "";
            cursoBox.Text = "";
            datainiPicker.Text = "";
            datafimPicker.Text = "";
            datacontratoPicker.Text = "";
            entradaBox.Text = "";
            saidaBox.Text = "";

            tabCadastrar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabVisualizar_Loaded(null, null);
        }
        

        private void tabAtualizar_Enable()
        {
            poloBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(poloBoxAtualizar, 0);
            Panel.SetZIndex(lblOverPolo, 1);

            instituicaoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(instituicaoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverInstituicao, 1);

            cursoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cursoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCurso, 1);

            datainiPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(datainiPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataInicio, 1);

            datafimPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(datafimPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataFim, 1);

            datacontratoPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(datacontratoPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataContrato, 1);

            entradaBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(entradaBoxAtualizar, 0);
            Panel.SetZIndex(lblOverEntrada, 1);

            saidaBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(saidaBoxAtualizar, 0);
            Panel.SetZIndex(lblOverSaida, 1);
        }

        private void lblOverPolo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            poloBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(poloBoxAtualizar, 1);
            Panel.SetZIndex(lblOverPolo, 0);
        }

        private void lblOverInstituicao_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            instituicaoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(instituicaoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverInstituicao, 0);
        }

        private void lblOverCurso_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cursoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cursoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCurso, 0);
        }

        private void lblOverDataInicio_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            datainiPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(datainiPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataInicio, 0);
        }

        private void lblOverDataFim_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            datafimPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(datafimPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataFim, 0);
        }

        private void lblOverDataContrato_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            datacontratoPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(datacontratoPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataContrato, 0);
        }

        private void lblOverEntrada_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            entradaBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(entradaBoxAtualizar, 1);
            Panel.SetZIndex(lblOverEntrada, 0);
        }

        private void lblOverSaida_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            saidaBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(saidaBoxAtualizar, 1);
            Panel.SetZIndex(lblOverSaida, 0);
        }

        private void poloBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (poloBoxAtualizar.IsEnabled == true)
            {
                poloBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(poloBoxAtualizar, 0);
                Panel.SetZIndex(lblOverPolo, 1);
            }
        }

        private void instituicaoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (instituicaoBoxAtualizar.IsEnabled == true)
            {
                instituicaoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(instituicaoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverInstituicao, 1);
            }
        }

        private void cursoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cursoBoxAtualizar.IsEnabled == true)
            {
                cursoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(cursoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCurso, 1);
            }
        }

        private void datainiPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (datainiPickerAtualizar.IsEnabled == true)
            {
                datainiPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(datainiPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataInicio, 1);
            }
        }

        private void datafimPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (datafimPickerAtualizar.IsEnabled == true)
            {
                datafimPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(datafimPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataFim, 1);
            }
        }

        private void datacontratoPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (datacontratoPickerAtualizar.IsEnabled == true)
            {
                datacontratoPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(datacontratoPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataContrato, 1);
            }
        }

        private void entradaBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (entradaBoxAtualizar.IsEnabled == true)
            {
                entradaBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(entradaBoxAtualizar, 0);
                Panel.SetZIndex(lblOverEntrada, 1);
            }
        }

        private void saidaBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (saidaBoxAtualizar.IsEnabled == true)
            {
                saidaBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(saidaBoxAtualizar, 0);
                Panel.SetZIndex(lblOverSaida, 1);
            }
        }

        private void tabAtualizar_Loaded(object sender, RoutedEventArgs e)
        {
            tabAtualizar_Enable();

            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT f.nome FROM rh.funcionario f "
                             + "INNER JOIN rh.estagiario e ON f.id = e.id;");

                estagiarios = new List<string>();

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                    estagiarios.Add(linha["nome"].ToString());
                nomeBoxAtualizar.ItemsSource = estagiarios;


                connection.SQL("SELECT nome FROM vc.polo ORDER BY nome ASC; ");
                polos = new List<string>();

                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                    polos.Add(linha["nome"].ToString());
                poloBoxAtualizar.ItemsSource = polos;
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void nomeBoxAtualizar_SelectionChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tabAtualizar_Enable();

            usuario = new LoginRole();
            try
            {
                string estagiario = nomeBoxAtualizar.SelectedValue.ToString();

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("EXEC rh.visualizar_estagiario_especifico '" + estagiario + "'; ");

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                {
                    poloBoxAtualizar.Text = linha["polo"].ToString();
                    instituicaoBoxAtualizar.Text = linha["instituicao"].ToString();
                    cursoBoxAtualizar.Text = linha["curso"].ToString();
                    datainiPickerAtualizar.Text = linha["inicio_curso"].ToString();
                    datafimPickerAtualizar.Text = linha["fim_curso"].ToString();
                    datacontratoPickerAtualizar.Text = linha["data_contrato"].ToString();
                    entradaBoxAtualizar.Text = linha["horario_entrada"].ToString();
                    saidaBoxAtualizar.Text = linha["horario_saida"].ToString();
                }
            }
            catch
            {
            }
        }

        private void atualizarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Estagiario estagiario = new Estagiario();
            contador_erros = 0;
            int contador_mudancas = 0;

            string sql_update = "";

            try
            {
                estagiario.Nome_funcionario = nomeBoxAtualizar.SelectedValue.ToString();
                lblNomeAtualizar.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNomeAtualizar.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (poloBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Nome_polo = poloBoxAtualizar.SelectedValue.ToString();
                    lblPoloAtualizar.Foreground = Brushes.Black;

                    sql_update += "UPDATE rh.estagiario SET polo = "
                                + "(SELECT id FROM vc.polo WHERE nome = '" + estagiario.Nome_polo + "')"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";

                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblPoloAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (instituicaoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Instituicao = instituicaoBoxAtualizar.Text;
                    lblInstituicaoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET instituicao = '" + estagiario.Instituicao + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblInstituicaoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cursoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Curso = cursoBoxAtualizar.Text;
                    lblCursoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET curso = '" + estagiario.Curso + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCursoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (datainiPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Inicio_curso = datainiPickerAtualizar.Text;
                    lblDataIniAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET inicio_curso = '" + estagiario.Inicio_curso + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataIniAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (datafimPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Fim_curso = datafimPickerAtualizar.Text;
                    lblDataFimAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET fim_curso = '" + estagiario.Fim_curso + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataFimAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (datacontratoPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Data_contrato = datacontratoPickerAtualizar.Text;
                    lblDataContratoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET data_contrato = '" + estagiario.Data_contrato + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataContratoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (entradaBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Horario_entrada = entradaBoxAtualizar.Text;
                    lblEntradaAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET horario_entrada = '" + estagiario.Horario_entrada + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblEntradaAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (saidaBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Horario_saida = saidaBoxAtualizar.Text;
                    lblSaidaAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.estagiario SET horario_saida = '" + estagiario.Horario_saida + "'"
                                + "WHERE id = "
                                + "(SELECT id FROM rh.funcionario "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "');";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblSaidaAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }
            
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
            Xceed.Wpf.Toolkit.MessageBox.Show("Estagiário atualizado.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);

            tabCadastrar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabVisualizar_Loaded(null, null);
        }



        private void tabRemover_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT f.nome FROM rh.funcionario f "
                             + "INNER JOIN rh.estagiario e ON f.id = e.id;");

                estagiarios = new List<string>();

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                    estagiarios.Add(linha["nome"].ToString());
                nomeBoxRemover.ItemsSource = estagiarios;
            }
            catch (Exception)
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
                string estagiario = nomeBoxRemover.SelectedValue.ToString();

                if (Xceed.Wpf.Toolkit.MessageBox.Show("Deseja remover o funcionário \n" + estagiario + " ?", "Remoção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();

                    string sql_delete = "EXEC rh.remover_estagiario '" + estagiario + "'";

                    connection.SQL(sql_delete);

                    Xceed.Wpf.Toolkit.MessageBox.Show("Estagiario removido com sucesso.\nLembre-se: O estagiário removido continua cadastrado como funcionário.");
                }
            }
            catch
            {
            }

            connection.Close();

            tabCadastrar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabVisualizar_Loaded(null, null);
        }



        private void tabVisualizar_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM rh.visualizar_estagiario ORDER BY nome ASC";
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                connection.SQL(sql);
                gridEstagiarios.DataContext = connection.View("estagiario");
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

        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                          + "Entre em contato com o desenvolvedor", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
