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
    public partial class Employer : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        int contador_erros;
        private List<string> funcionarios;

        public Employer()
        {
            InitializeComponent();
            usuario = new LoginRole();
        }

        private void cadastrarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionario = new Funcionario();
            Telefone telefone = new Telefone();
            Login login = new Login();
            
            contador_erros = 0;

            try
            {
                funcionario.Nome = nomeBox.Text;
                lblNome.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNome.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Cpf = cpfBox.Text;
                lblCpf.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCpf.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Rg = rgBox.Text;
                lblRg.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblRg.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Email = emailBox.Text;
                lblEmail.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblEmail.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Sexo = char.Parse(sexoBox.Text);
                lblSexo.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSexo.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                telefone.Tel = telefoneBox.Text;
                lblTelefone.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblTelefone.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Data_nasc = datanascPicker.Text;
                lblDataNasc.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblDataNasc.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Logradouro = logradouroBox.Text;
                lblLogradouro.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblLogradouro.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Numero = Int32.Parse(numeroBox.Text);
                lblNumero.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNumero.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Complemento = complementoBox.Text;
                lblComplemento.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblComplemento.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Cep = cepBox.Text;
                lblCep.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCep.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Cidade = cidadeBox.Text;
                lblCidade.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCidade.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Cargo = cargoBox.Text;
                lblCargo.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCargo.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                funcionario.Data_admissao = dataadmissaoPicker.Text;
                lblDataAdmissao.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblDataAdmissao.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                login.Username = loginBox.Text;
                lblLogin.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblLogin.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                login.Password = senhaBox.Text;
                lblSenha.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSenha.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            int check = 0;

            if (radioSuper.IsChecked == true)
                check = 1; 
            
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql_insert = "EXEC rh.cadastrar_funcionario "
                                  + "'"    + funcionario.Cpf
                                  + "', '" + funcionario.Rg
                                  + "', '" + funcionario.Nome
                                  + "', '" + funcionario.Email
                                  + "', '" + funcionario.Data_nasc
                                  + "', '" + funcionario.Sexo
                                  + "', '" + funcionario.Logradouro
                                  + "',  " + funcionario.Numero
                                  + " , '" + funcionario.Complemento
                                  + "', '" + funcionario.Cep
                                  + "', '" + funcionario.Cidade
                                  + "', '" + funcionario.Cargo
                                  + "', '" + funcionario.Data_admissao
                                  + "', '" + telefone.Tel
                                  + "', '" + login.Username
                                  + "', '" + login.Password
                                  + "',  " + check
                                  + " ;";

                connection.SQL(sql_insert);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();
            Xceed.Wpf.Toolkit.MessageBox.Show("Funcionário cadastrado.", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);

            nomeBox.Text = "";
            cpfBox.Text = "";
            rgBox.Text = "";
            emailBox.Text = "";
            sexoBox.Text = "";
            telefoneBox.Text = "";
            datanascPicker.Text = "";
            logradouroBox.Text = "";
            numeroBox.Text = "";
            complementoBox.Text = "";
            cepBox.Text = "";
            cidadeBox.Text = "";
            cargoBox.Text = "";
            dataadmissaoPicker.Text = "";
            loginBox.Text = "";
            senhaBox.Text = "";

            tabVisualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabAtualizar_Loaded(null, null);

            if (check == 0)
            {
                Intern intern = new Intern();
                intern.tabCadastrar.Focus();
                intern.estagiario_novo = funcionario.Nome;
                intern.ShowDialog();
            }   
        }


        // isEnable = false
        private void tabAtualizar_Enable()
        {
            nomeTextBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(nomeBoxAtualizar, 0);
            Panel.SetZIndex(nomeTextBoxAtualizar, 1);
            Panel.SetZIndex(lblOverNome, 2);

            cpfBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cpfBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCpf, 1);

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

            cargoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(cargoBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCargo, 1);

            dataadmissaoPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(dataadmissaoPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataAdmissao, 1);

            datademissaoPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(datademissaoPickerAtualizar, 0);
            Panel.SetZIndex(lblOverDataDemissao, 1);

            loginAtualizarBox.IsEnabled = false;

            radioSuperAtualizar.IsEnabled = false;
            Panel.SetZIndex(radioSuperAtualizar, 0);
            Panel.SetZIndex(lblOverSuper, 1);

            radioInterAtualizar.IsEnabled = false;
            Panel.SetZIndex(radioInterAtualizar, 0);
            Panel.SetZIndex(lblOverInter, 1);
        }

        // isEnable = true
        private void lblOverNome_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            nomeTextBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(nomeBoxAtualizar, 1);
            Panel.SetZIndex(nomeTextBoxAtualizar, 2);
            Panel.SetZIndex(lblOverNome, 0);
        }

        private void lblOverCpf_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cpfBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cpfBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCpf, 0);            
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

        private void lblOverCargo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cargoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(cargoBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCargo, 0);
        }

        private void lblOverDataAdmissao_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dataadmissaoPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(dataadmissaoPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataAdmissao, 0);
        }

        private void lblOverDataDemissao_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            datademissaoPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(datademissaoPickerAtualizar, 1);
            Panel.SetZIndex(lblOverDataDemissao, 0);
        }

        private void lblOverSuper_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            radioSuperAtualizar.IsEnabled = true;
            Panel.SetZIndex(radioSuperAtualizar, 1);
            Panel.SetZIndex(lblOverSuper, 0);
        }
        
        private void lblOverInter_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            radioInterAtualizar.IsEnabled = true;
            Panel.SetZIndex(radioInterAtualizar, 1);
            Panel.SetZIndex(lblOverInter, 0);
        }

        private void nomeTextBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (nomeTextBoxAtualizar.IsEnabled == true)
            {
                nomeTextBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(nomeBoxAtualizar, 0);
                Panel.SetZIndex(nomeTextBoxAtualizar, 1);
                Panel.SetZIndex(lblOverNome, 2);
            }
        }

        private void cpfBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cpfBoxAtualizar.IsEnabled == true)
            {
                cpfBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(cpfBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCpf, 1);
            }
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

        private void cargoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cargoBoxAtualizar.IsEnabled == true)
            {
                cargoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(cargoBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCargo, 1);
            }
        }

        private void dataadmissaoPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataadmissaoPickerAtualizar.IsEnabled == true)
            {
                dataadmissaoPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(dataadmissaoPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataAdmissao, 1);
            }
        }

        private void datademissaoPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (datademissaoPickerAtualizar.IsEnabled == true)
            {
                datademissaoPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(datademissaoPickerAtualizar, 0);
                Panel.SetZIndex(lblOverDataDemissao, 1);
            }
        }

        private void radioSuperAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (radioSuperAtualizar.IsEnabled == true)
            {
                radioSuperAtualizar.IsEnabled = false;
                Panel.SetZIndex(radioSuperAtualizar, 0);
                Panel.SetZIndex(lblOverSuper, 1);
            }
        }

        private void radioInterAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (radioInterAtualizar.IsEnabled == true)
            {
                radioInterAtualizar.IsEnabled = false;
                Panel.SetZIndex(radioInterAtualizar, 0);
                Panel.SetZIndex(lblOverInter, 1);
            }
        }

        
        private void tabAtualizar_Loaded(object sender, RoutedEventArgs e)
        {
            tabAtualizar_Enable();

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT nome FROM rh.funcionario ORDER BY nome ASC");

                funcionarios = new List<string>();

                foreach (DataRow linha in connection.View("funcionario").Tables["funcionario"].Rows)
                    funcionarios.Add(linha["nome"].ToString());

                nomeBoxAtualizar.ItemsSource = funcionarios;
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
            
            Login login = new Login();
            try
            {
                string funcionario = nomeBoxAtualizar.SelectedValue.ToString();

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("EXEC rh.visualizar_funcionario_especifico '" + funcionario + "'; ");

                foreach (DataRow linha in connection.View("funcionario").Tables["funcionario"].Rows)
                {
                    nomeTextBoxAtualizar.Text = linha["nome"].ToString();
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
                    cargoBoxAtualizar.Text = linha["cargo"].ToString();
                    dataadmissaoPickerAtualizar.Text = linha["data_admissao"].ToString();
                    datademissaoPickerAtualizar.Text = linha["data_demissao"].ToString();
                    loginAtualizarBox.Text = linha["username"].ToString();

                    if (linha["permissao"].ToString().Equals("S"))
                    {
                        login.Permissao = "S";
                        radioSuperAtualizar.IsChecked = true;
                    }
                    else
                    {
                        login.Permissao = "E";
                        radioInterAtualizar.IsChecked = true;
                    }
                }
            }
            catch
            {
            }
        }
        
        private void atualizarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionario = new Funcionario();
            Telefone telefone = new Telefone();
            Login login = new Login();

            contador_erros = 0;
            int contador_mudancas = 0;
            string sql_update = null;

            if (nomeTextBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Nome = nomeTextBoxAtualizar.Text;
                    lblNomeAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET nome = '" + funcionario.Nome + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblNomeAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cpfBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Cpf = cpfBoxAtualizar.Text;
                    lblCpfAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cpf = '" + funcionario.Cpf + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCpfAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (rgBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Rg = rgBoxAtualizar.Text;
                    lblRgAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET rg = '" + funcionario.Rg + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Email = emailBoxAtualizar.Text;
                    lblEmailAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET email = '" + funcionario.Email + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Sexo = char.Parse(sexoBoxAtualizar.Text);
                    lblSexoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET sexo = '" + funcionario.Sexo + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                                + "WHERE f.nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'); ";
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
                    funcionario.Data_nasc = datanascPickerAtualizar.Text;
                    lblDataNascAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET data_nasc = '" + funcionario.Data_nasc + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Logradouro = logradouroBoxAtualizar.Text;
                    lblLogradouroAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET logradouro = '" + funcionario.Logradouro + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Numero = Int32.Parse(numeroBoxAtualizar.Text);
                    lblNumeroAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET numero = '" + funcionario.Numero + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Complemento = complementoBoxAtualizar.Text;
                    lblComplementoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET complemento = '" + funcionario.Complemento + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Cep = cepBoxAtualizar.Text;
                    lblCepAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cep = '" + funcionario.Cep + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
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
                    funcionario.Cidade = cidadeBoxAtualizar.Text;
                    lblCidadeAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cidade = '" + funcionario.Cidade + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCidadeAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (cargoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Cargo = cargoBoxAtualizar.Text;
                    lblCargoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET cargo = '" + funcionario.Cargo + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblCargoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (dataadmissaoPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Data_admissao = dataadmissaoPickerAtualizar.Text;
                    lblDataAdmissaoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET data_admissao = '" + funcionario.Data_admissao + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataAdmissaoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (datademissaoPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    funcionario.Data_demissao = datademissaoPickerAtualizar.Text;
                    lblDataDemissaoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE rh.funcionario "
                                + "SET data_demissao = '" + funcionario.Data_demissao + "' "
                                + "WHERE nome = '" + nomeBoxAtualizar.SelectedValue.ToString() + "'; ";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataDemissaoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if ((radioSuperAtualizar.IsEnabled == true) && (radioSuperAtualizar.IsChecked == true))
            {
                sql_update += "EXEC rh.estagiario_to_supervisor '" + loginAtualizarBox.Text + "'; ";
                contador_mudancas++;
            }

            if ((radioInterAtualizar.IsEnabled == true) && (radioInterAtualizar.IsChecked == true))
            {
                sql_update += "EXEC rh.supervisor_to_estagiario '" + loginAtualizarBox.Text + "'; ";
                contador_mudancas++;
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
            Xceed.Wpf.Toolkit.MessageBox.Show("Funcionário atualizado.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);

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

                connection.SQL("SELECT nome FROM rh.funcionario ORDER BY nome ASC");

                funcionarios = new List<string>();

                foreach (DataRow linha in connection.View("funcionario").Tables["funcionario"].Rows)
                    funcionarios.Add(linha["nome"].ToString());

                nomeComboBox.ItemsSource = funcionarios;
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
                string funcionario = nomeComboBox.SelectedValue.ToString();

                if (Xceed.Wpf.Toolkit.MessageBox.Show("Deseja remover o funcionário \n" + funcionario + " ?", "Remoção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();

                    string sql_delete = "EXEC rh.remover_funcionario "
                                      + "'" + funcionario + "'; ";

                    connection.SQL(sql_delete);

                    Xceed.Wpf.Toolkit.MessageBox.Show("Funcionário removido com sucesso.");
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
            string sql = "BEGIN TRANSACTION; "
                       + "SELECT * FROM rh.visualizar_funcionario; "
                       + "COMMIT;";

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                connection.SQL(sql);
                gridFuncionarios.DataContext = connection.View("funcionario");
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





  
    }
}
