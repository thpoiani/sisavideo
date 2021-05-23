using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MahApps.Metro.Controls;
using System.Linq;

namespace SisAVideo.Supervisor
{
    public partial class ActivatedPolo : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;

        private List<string> polos_cadastrados;
        private List<string> polos_ativos;
        private List<string> polos_ativos_bkp;
        
        public List<string> polos;


        public ActivatedPolo(List<string> polos)
        {
            InitializeComponent();
            polos_ativos_bkp = polos.ToList();
        }

        private void Polo_Loaded(object sender, RoutedEventArgs e)
        {
            polos_cadastrados = new List<string>();
            polos_ativos = new List<string>();

            polos_ativos = polos_ativos_bkp;
            
            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT nome FROM vc.polo ORDER BY nome ASC");
            
                foreach (DataRow linha in connection.View("polo").Tables["polo"].Rows)
                    polos_cadastrados.Add(linha["nome"].ToString());
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();

            polos_cadastrados = polos_cadastrados.Except(polos_ativos).ToList();

            Sort();
        }

        private void Sort()
        {
            polos_cadastrados.Sort();
            polos_ativos.Sort();

            ativosBox.ItemsSource = null;
            ativosBox.ItemsSource = polos_ativos;

            cadastradosBox.ItemsSource = null;
            cadastradosBox.ItemsSource = polos_cadastrados;
        }

        private void ativarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                polos_ativos.Add(cadastradosBox.SelectedValue.ToString());
                polos_cadastrados.Remove(cadastradosBox.SelectedValue.ToString());
            }
            catch
            {
                cadastradosBox.ItemsSource = polos_cadastrados;
                ativosBox.ItemsSource = polos_ativos;
            }
            finally
            {
                Sort();
            }
        }

        private void removerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                polos_cadastrados.Add(ativosBox.SelectedValue.ToString());
                polos_ativos.Remove(ativosBox.SelectedValue.ToString());
            }
            catch
            {
                cadastradosBox.ItemsSource = polos_cadastrados;
                ativosBox.ItemsSource = polos_ativos;
            }
            finally
            {
                Sort();
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string polo_qtd;
            
            switch (ativosBox.Items.Count)
            {
                case 0: polo_qtd = "Nenhum polo atribuído"; break;
                case 1: polo_qtd = "Polo atribuído"; break;
                default: polo_qtd = "Polos atribuídos"; break;
            }

            polos = polos_ativos.ToList();

            Xceed.Wpf.Toolkit.MessageBox.Show(polo_qtd + " à Videoconferência.", "Polos ativados", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            polos = polos_ativos_bkp.ToList();

            this.DialogResult = false;
            this.Close();
        }

        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                                            + "Entre em contato com o desenvolvedor.", "Erro",
                                              MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
