using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace SisAVideo.Intern
{
    public partial class Employer : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario = new LoginRole();

        public Employer()
        {
            InitializeComponent();
        }

        private void tabEstagiario_Enable()
        {
            lblNomeAtualizar.Foreground = Brushes.LightGray;
            lblCpfAtualizar.Foreground = Brushes.LightGray;
            
            rgBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(rgBoxAtualizar, 0);
            Panel.SetZIndex(lblOverRg, 1);

            emailBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(emailBoxAtualizar, 0);
            Panel.SetZIndex(lblOverEmail, 1);


            sexoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(sexoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverSexo, 1);

            telefoneBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(telefoneBoxAtualizar, 0);
            Panel.SetZIndex(lblOverTelefone, 1);

            datanascPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(datanascPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataNasc, 1);

            logradouroBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(logradouroBoxAtualizar, 0);
            Panel.SetZIndex(lblOverLogradouro, 1);

            numeroBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(numeroBoxAtualizar, 0);
            Panel.SetZIndex(lblOverNumero, 1);

            complementoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(complementoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverComplemento, 1);

            cepBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cepBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCep, 1);

            cidadeBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cidadeBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCidade, 1);

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
        }

        private void tabEstagiario_Loaded(object sender, RoutedEventArgs e)
        {
            tabEstagiario_Enable();
            
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                connection.SQL("EXEC rh.estagiario_dados '" + usuario.Username + "'");

                foreach (DataRow linha in connection.View("estagiario").Tables["estagiario"].Rows)
                {
                    nomeBoxAtualizar.Text = linha["nome"].ToString();
                    cpfBoxAtualizar.Text = linha["cpf"].ToString();
                    rgBoxAtualizar.Text = linha["rg"].ToString();
                    emailBoxAtualizar.Text = linha["email"].ToString();
                    sexoBoxAtualizar.Text = linha["sexo"].ToString();
                    telefoneBoxAtualizar.Text = linha["telefone"].ToString();
                    datanascPickerAtualizar.Text = linha["data_nasc"].ToString();
                    logradouroBoxAtualizar.Text = linha["logradouro"].ToString();
                    numeroBoxAtualizar.Text = linha["numero"].ToString();
                    complementoBoxAtualizar.Text = linha["complemento"].ToString();
                    cepBoxAtualizar.Text = linha["cep"].ToString();
                    cidadeBoxAtualizar.Text = linha["cidade"].ToString();
                    instituicaoBoxAtualizar.Text = linha["instituicao"].ToString();
                    cursoBoxAtualizar.Text = linha["curso"].ToString();
                    datainiPickerAtualizar.Text = linha["inicio_curso"].ToString();
                    datafimPickerAtualizar.Text = linha["fim_curso"].ToString();
                }
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void lblOverRg_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            rgBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(rgBoxAtualizar, 1);
            Panel.SetZIndex(lblOverRg, 0);
        }

        private void lblOverEmail_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            emailBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(emailBoxAtualizar, 1);
            Panel.SetZIndex(lblOverEmail, 0);
        }

        private void lblOverSexo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            sexoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(sexoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverSexo, 0);
        }

        private void lblOverTelefone_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            telefoneBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(telefoneBoxAtualizar, 1);
            Panel.SetZIndex(lblOverTelefone, 0);
        }

        private void lblOverDataNasc_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            datanascPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(datanascPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataNasc, 0);
        }

        private void lblOverLogradouro_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            logradouroBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(logradouroBoxAtualizar, 1);
            Panel.SetZIndex(lblOverLogradouro, 0);
        }

        private void lblOverNumero_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            numeroBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(numeroBoxAtualizar, 1);
            Panel.SetZIndex(lblOverNumero, 0);
        }

        private void lblOverComplemento_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            complementoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(complementoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverComplemento, 0);
        }

        private void lblOverCep_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cepBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cepBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCep, 0);
        }

        private void lblOverCidade_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cidadeBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cidadeBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCidade, 0);
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

        private void rgBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (rgBoxAtualizar.IsEnabled == true)
            {
                rgBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(rgBoxAtualizar, 0);
                Panel.SetZIndex(lblOverRg, 1);
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

        private void sexoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sexoBoxAtualizar.IsEnabled == true)
            {
                sexoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(sexoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverSexo, 1);
            }
        }

        private void telefoneBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (telefoneBoxAtualizar.IsEnabled == true)
            {
                telefoneBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(telefoneBoxAtualizar, 0);
                Panel.SetZIndex(lblOverTelefone, 1);
            }
        }

        private void datanascPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (datanascPickerAtualizar.IsEnabled == true)
            {
                datanascPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(datanascPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataNasc, 1);
            }
        }

        private void logradouroBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (logradouroBoxAtualizar.IsEnabled == true)
            {
                logradouroBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(logradouroBoxAtualizar, 0);
                Panel.SetZIndex(lblOverLogradouro, 1);
            }
        }

        private void numeroBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (numeroBoxAtualizar.IsEnabled == true)
            {
                numeroBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(numeroBoxAtualizar, 0);
                Panel.SetZIndex(lblOverNumero, 1);
            }
        }

        private void complementoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (complementoBoxAtualizar.IsEnabled == true)
            {
                complementoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(complementoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverComplemento, 1);
            }
        }

        private void cepBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cepBoxAtualizar.IsEnabled == true)
            {
                cepBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(cepBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCep, 1);
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

        private void atualizarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Estagiario estagiario = new Estagiario();
            Telefone telefone = new Telefone();

            int contador_erros = 0;
            int contador_mudancas = 0;
            string sql_update = null;

            estagiario.Nome_funcionario = nomeBoxAtualizar.Text;

            if (rgBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Rg = rgBoxAtualizar.Text;
                    lblRgAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET rg = '" + estagiario.Rg + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblRgAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (emailBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Email = emailBoxAtualizar.Text;
                    lblEmailAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET email = '" + estagiario.Email + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblEmailAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (sexoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Sexo = char.Parse(sexoBoxAtualizar.Text);
                    lblSexoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET sexo = '" + estagiario.Sexo + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblSexoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (telefoneBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    telefone.Tel = telefoneBoxAtualizar.Text;
                    lblTelefoneAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.telefone "
                                + "SET telefone = '" + telefone.Tel + "' "
                                + "WHERE id = "
                                + "(SELECT t.id FROM rh.telefone t INNER JOIN rh.funcionario f "
                                + "ON t.id = f.id "
                                + "WHERE f.nome = '" + estagiario.Nome_funcionario + "'); ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblTelefoneAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (datanascPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Data_nasc = datanascPickerAtualizar.Text;
                    lblDataNascAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET data_nasc = '" + estagiario.Data_nasc + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataNascAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (logradouroBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Logradouro = logradouroBoxAtualizar.Text;
                    lblLogradouroAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET logradouro = '" + estagiario.Logradouro + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblLogradouroAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (numeroBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Numero = Int32.Parse(numeroBoxAtualizar.Text);
                    lblNumeroAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET numero = '" + estagiario.Numero + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblNumeroAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (complementoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Complemento = complementoBoxAtualizar.Text;
                    lblComplementoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET complemento = '" + estagiario.Complemento + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblComplementoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cepBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Cep = cepBoxAtualizar.Text;
                    lblCepAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cep = '" + estagiario.Cep + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCepAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cidadeBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    estagiario.Cidade = cidadeBoxAtualizar.Text;
                    lblCidadeAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cidade = '" + estagiario.Cidade + "' "
                                + "WHERE nome = '" + estagiario.Nome_funcionario + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCidadeAtualizar.Foreground = Brushes.Red;
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

            tabEstagiario_Loaded(null, null);
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
