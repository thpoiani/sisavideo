using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;

namespace SisAVideo.Intern
{
    public partial class Videoconference : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;

        public Videoconference()
        {
            InitializeComponent();
        }

        private void Videoconference_Loaded(object sender, RoutedEventArgs e)
        {   
            string email = null;

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);

                connection.Open();

                email = connection.ReturnString("EXEC vc.polo_email '" + usuario.Username + "'");
            }
            catch
            {
                erro();
                Application.Current.Shutdown();
            }


            string html = "<html><head><style> html {overflow: auto;}  "
                        + "body {overflow: hidden;} </style></head><body>"
                        + "<iframe src=\"https://www.google.com/calendar/embed?"
                        + "showTitle=0&amp;showNav=0&amp;showDate=0&amp;showPrint=0&amp;"
                        + "showTabs=0&amp;showCalendars=0&amp;showTz=0&amp;"
                        + "height=600&amp;wkst=1&amp;bgcolor=%23FFFFFF&amp;"
                        + "src=" + email + "&amp;"
                        + "color=%232952A3&amp;color=%232F6309&amp;color=%23875509&amp;"
                        + "ctz=America%2FSao_Paulo\" style=\" border-width:0 \""
                        + "width=\"580\" height=\"370\" frameborder=\"0\" scrolling=\"no\">"
                        + "</iframe></body></html>";

            browser.NavigateToString(html);
        }

        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                                            + "Entre em contato com o desenvolvedor.", "Erro",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
