using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace SisAVideo.Supervisor
{ 
    public partial class Target : MetroWindow
    {
        private int contador_erros;
        private List<string> alvos;
        public string retorno;

        public Target(string sqlTarget)
        {
            InitializeComponent();

            alvos = new List<string>();
            alvos.Add("Assistentes");
            alvos.Add("Coordenadores");
            alvos.Add("Diretores");
            alvos.Add("Profissionais da área");
            alvos.Add("Técnicos");
            alvos.Add("Outros");

            targetBox.ItemsSource = alvos;

            if (!string.IsNullOrEmpty(sqlTarget))
                if ((sqlTarget == "Assistentes") || (sqlTarget == "Coordenadores")
                   || (sqlTarget == "Diretores") || (sqlTarget == "Profissionais da área")
                   || (sqlTarget == "Técnicos"))
                    targetBox.SelectedItem = sqlTarget;
                else
                {
                    targetBox.SelectedItem = "Outros";
                    outroBox.Text = sqlTarget;
                }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            PublicoAlvo target = new PublicoAlvo();
            
            contador_erros = 0;

            try
            {
                if (targetBox.SelectedValue.ToString() == "Outros")
                {
                    target.Publicoalvo = outroBox.Text;
                }
                else
                {
                    target.Publicoalvo = targetBox.SelectedValue.ToString();
                }
                lblTarget.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblTarget.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            retorno = target.Publicoalvo;
            
            Xceed.Wpf.Toolkit.MessageBox.Show("Público Alvo atribuído à Videoconferência.", "Público Alvo", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();   
        }

        private void targetBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (targetBox.SelectedValue.ToString() == "Outros")
            {
                outroBox.Visibility = Visibility.Visible;
            }
            else
            {
                outroBox.Text = "";
                outroBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
