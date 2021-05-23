using System.Reflection;
using MahApps.Metro.Controls;

namespace SisAVideo.Common
{
    public partial class About : MetroWindow
    {
        private object[] attributes;

        public About()
        {
            InitializeComponent();

            lblTitulo.Content = "SisAVideo";

            //
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            AssemblyTitleAttribute atributo_title = (AssemblyTitleAttribute)attributes[0];
            lblPrograma.Content = atributo_title.Title;
            
            //
            lblVersao.Content = "Versão " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            AssemblyCopyrightAttribute atributo_copyright = (AssemblyCopyrightAttribute)attributes[0];
            lblCopyright.Content = atributo_copyright.Copyright;

            //

            lblRights.Content = "Todos os direitos reservados.";

            //
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            AssemblyDescriptionAttribute atributo_description = (AssemblyDescriptionAttribute)attributes[0];
            lblDescricao.Text = atributo_description.Description;

            //
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            AssemblyCompanyAttribute atributo_compania = (AssemblyCompanyAttribute)attributes[0];
            lblDescricao.Text += "\n                               " + atributo_compania.Company;
            
            
        }       
    }
}
