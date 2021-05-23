using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;

namespace SisAVideo.Supervisor
{
    public partial class Lecture : MetroWindow
    {
        public Dictionary<string, string> conferencistas;
        private int contador_erros;
        private string cpf;
        private string nome;

        public Lecture(Dictionary<string, string> conferencistas)
        {
            InitializeComponent();
            this.conferencistas = conferencistas;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Conferencista conferencista = new Conferencista();

            contador_erros = 0;

            try
            {
                conferencista.Cpf = cpfBox.Text;
                lblCpf.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCpf.Foreground = Brushes.Red;
                contador_erros++;
            }

            foreach (KeyValuePair<string, string> tupla in conferencistas)
            {
                if (tupla.Key == conferencista.Cpf)
                {
                    lblCpf.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            try
            {
                conferencista.Nome = nomeBox.Text;
                lblNome.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblNome.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            conferencistas.Add(conferencista.Cpf, conferencista.Nome);
            

            Xceed.Wpf.Toolkit.MessageBox.Show("Conferencista atribuído à Videoconferência.", "Conferencista", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void tabAtualizar_Enable()
        {
            cpfBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(lblCpfBoxAtualizar, 0);
            Panel.SetZIndex(lblOverCpf, 1);

            nomeBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(nomeBoxAtualizar, 1);
            Panel.SetZIndex(nomeComboBoxAtualizar, 0);
            Panel.SetZIndex(lblOverNome, 2);
        }

        private void lblOverNome_Content_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            nomeBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(nomeBoxAtualizar, 2);
            Panel.SetZIndex(nomeComboBoxAtualizar, 1);
            Panel.SetZIndex(lblOverNome, 0);
        }

        private void lblOverCpf_Content_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cpfBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(lblCpfBoxAtualizar, 1);
            Panel.SetZIndex(lblOverCpf, 0);
        }

        private void nomeBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (nomeBoxAtualizar.IsEnabled == true)
            {
                nomeBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(nomeBoxAtualizar, 1);
                Panel.SetZIndex(nomeComboBoxAtualizar, 0);
                Panel.SetZIndex(lblOverNome, 2);
            }
        }

        private void cpfBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cpfBoxAtualizar.IsEnabled == true)
            {
                cpfBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(lblCpfBoxAtualizar, 0);
                Panel.SetZIndex(lblOverCpf, 1);
            }
        }

        private void tabAtualizar_Loaded(object sender, RoutedEventArgs e)
        {
            tabAtualizar_Enable();

            List<string> nomes = new List<string>();

            foreach (KeyValuePair<string, string> tupla in conferencistas)
                nomes.Add(tupla.Value);

            nomeComboBoxAtualizar.ItemsSource = nomes;
        }

        private void nomeComboBoxAtualizar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (KeyValuePair<string, string> tupla in conferencistas)
            {
                if (tupla.Value == nomeComboBoxAtualizar.SelectedValue.ToString())
                {
                    cpf = cpfBoxAtualizar.Text = tupla.Key;
                    nome = nomeBoxAtualizar.Text = tupla.Value;
                }
            }
        }

        private void okButtonAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Conferencista conferencista = new Conferencista();

            contador_erros = 0;
            int contador_mudancas = 0;

            if (cpfBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    conferencista.Cpf = cpfBoxAtualizar.Text;
                    lblCpfAtualizar.Foreground = Brushes.Black;
                }
                catch (Exception)
                {
                    lblCpfAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }

                foreach (KeyValuePair<string, string> tupla in conferencistas)
                {
                    if (tupla.Key == conferencista.Cpf)
                    {
                        lblCpfAtualizar.Foreground = Brushes.Red;
                        contador_erros++;
                    }
                }

                if (contador_erros != 0)
                    return;

                contador_mudancas++;
            }

            if (nomeBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    conferencista.Nome = nomeBoxAtualizar.Text;
                    lblNomeAtualizar.Foreground = Brushes.Black;
                }
                catch (Exception)
                {
                    lblNomeAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
                contador_mudancas++;
            }

            if (contador_erros != 0)
                return;

            if (contador_mudancas == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Nenhuma alteração.", "Conferencista", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            conferencistas.Remove(cpf);

            if ((cpfBoxAtualizar.IsEnabled == true) && (nomeBoxAtualizar.IsEnabled == true))
                conferencistas.Add(conferencista.Cpf, conferencista.Nome);
            else
                if ((cpfBoxAtualizar.IsEnabled == true) && (nomeBoxAtualizar.IsEnabled == false))
                    conferencistas.Add(conferencista.Cpf, nome);
                else
                    if ((cpfBoxAtualizar.IsEnabled == false) && (nomeBoxAtualizar.IsEnabled == true))
                        conferencistas.Add(cpf, conferencista.Nome);

            Xceed.Wpf.Toolkit.MessageBox.Show("Conferencista atualizado.", "Conferencista", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.DialogResult = true;
            this.Close();
        }

        private void tabRemover_Loaded(object sender, RoutedEventArgs e)
        {
            cpfBoxRemover.ItemsSource = conferencistas.Keys;
        }

        private void okButtonRemover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cpf = cpfBoxRemover.SelectedValue.ToString();

                if (Xceed.Wpf.Toolkit.MessageBox.Show("Deseja remover o conferencista \n"
                    + conferencistas[cpf] + "?", "Remoção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    conferencistas.Remove(cpf);
                    Xceed.Wpf.Toolkit.MessageBox.Show("Conferencista removido.");
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch
            {
                return;
            }

        }

        private void tabVisualizar_Loaded(object sender, RoutedEventArgs e)
        {
            gridConferencistas.ItemsSource = conferencistas;
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
