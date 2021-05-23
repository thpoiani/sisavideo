using System;
using System.Data.SqlClient;
using System.Windows;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;

namespace SisAVideo.Intern
{

    public partial class Appointment : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private Apontamento apontamento = new Apontamento();
        private int verificador = 0;
        private bool apontamento_entrada;
        private bool apontamento_saida;
        

        public Appointment()
        {
            InitializeComponent();

            string sql_hour = "SELECT DATEPART(HOUR, CURRENT_TIMESTAMP)";
            string sql_minute = "SELECT DATEPART(MINUTE, CURRENT_TIMESTAMP)";
            string sql_second = "SELECT DATEPART(SECOND, CURRENT_TIMESTAMP)";

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
                apontamento.Agora = new TimeSpan(connection.Return(sql_hour), connection.Return(sql_minute), connection.Return(sql_second));
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            apontamento.Manha_inicio_entrada = new TimeSpan(6, 55, 0);
            apontamento.Manha_final_entrada = new TimeSpan(7, 05, 0);

            apontamento.Manha_inicio_saida = new TimeSpan(12, 55, 0);
            apontamento.Manha_final_saida = new TimeSpan(13, 05, 0);

            apontamento.Tarde_inicio_entrada = new TimeSpan(12, 25, 0);
            apontamento.Tarde_final_entrada = new TimeSpan(12, 35, 0);

            apontamento.Tarde_inicio_saida = new TimeSpan(18, 25, 0);
            apontamento.Tarde_final_saida = new TimeSpan(18, 35, 0);

            apontamento_entrada = (((apontamento.Agora > apontamento.Manha_inicio_entrada) && (apontamento.Agora < apontamento.Manha_final_entrada)) || ((apontamento.Agora > apontamento.Tarde_inicio_entrada) && (apontamento.Agora < apontamento.Tarde_final_entrada)));

            apontamento_saida = (((apontamento.Agora > apontamento.Manha_inicio_saida) && (apontamento.Agora < apontamento.Manha_final_saida)) || ((apontamento.Agora > apontamento.Tarde_inicio_saida) && (apontamento.Agora < apontamento.Tarde_final_saida)));
        }

         
        private void Appointment_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql = "SELECT rh.apontamento_verificador ('" + usuario.Username + "')";

                verificador = connection.Return(sql);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            switch (verificador)
            {
                case 2:
                    {
                        lblApontamento.Content = "Apontamentos de Entrada e Saída já realizados"; 
                        break;
                    }
                case 1:
                    {
                        lblApontamento.Content = "Apontamento de Saída";
                        if (!apontamento_saida)
                            justificativaBox.Visibility = Visibility.Visible;
                        break;
                    }
                case 0:
                    {
                        lblApontamento.Content = "Apontamento de Entrada";
                        if (!apontamento_entrada)
                            justificativaBox.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            switch (verificador)
            {
                case 2:
                    {
                        this.Close(); break;
                    }
                case 1:
                    {
                        if (apontamento_saida)
                            apontamentoSaida(apontamento.Agora);
                        else
                            apontamentoSaidaJustificado(apontamento.Agora, justificativaBox.Text);
                        break;
                    }
                case 0:
                    {
                        if (apontamento_entrada)
                            apontamentoEntrada(apontamento.Agora);
                        else
                            apontamentoEntradaJustificado(apontamento.Agora, justificativaBox.Text);
                        break;
                    }
            }
        }


        private void apontamentoEntrada(TimeSpan agora)
        {
            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql = "EXEC rh.apontamento_entrada '" + usuario.Username + "', null";
                connection.SQL(sql);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            sucesso("Entrada");
            this.Close();
        }

        private void apontamentoEntradaJustificado(TimeSpan agora, string justificativa)
        {
            if (string.IsNullOrEmpty(justificativa))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não deixe o campo de justificativa vazio.", "Justificativa", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql = "EXEC rh.apontamento_entrada '" +usuario.Username+ "',"
                           + "'ENTRADA: " +justificativa+ " '";
                connection.SQL(sql);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            sucesso("Entrada");
            this.Close();
        }
        
        private void apontamentoSaida(TimeSpan agora)
        {
            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql = "EXEC rh.apontamento_saida '" + usuario.Username + "', null";
                connection.SQL(sql);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            sucesso("Saída");
            this.Close();
        }

        private void apontamentoSaidaJustificado(TimeSpan agora, string justificativa)
        {
            if (string.IsNullOrEmpty(justificativa))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não deixe o campo de justificativa vazio.", "Justificativa", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql = "EXEC rh.apontamento_saida '" + usuario.Username + "', "
                           + "'SAIDA: " + justificativa + "'";
                connection.SQL(sql);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            sucesso("Saída");
            this.Close();
        }

        private void sucesso(string periodo)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Apontamento de " + periodo + " realizado com sucesso.", "Apontamento", MessageBoxButton.OK, MessageBoxImage.Information);
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
