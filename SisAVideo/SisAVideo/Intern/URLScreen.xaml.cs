using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;

namespace SisAVideo.Intern
{
    public partial class URLScreen : MetroWindow
    {
        protected static Regex _link = new Regex(@"^http:\/\/media.escolasdegoverno.sp.gov.br\/..*$");

        public URLScreen()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string streaming = streamingText.Text;

            if (!_link.IsMatch(streaming))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Streaming inválida. Use por exemplo:\n"
                + "http://media.escolasdegoverno.sp.gov.br/streaming", "Streaming", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Process.Start("wmplayer.exe", streaming);
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}