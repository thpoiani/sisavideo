using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace SisAVideo
{
    public partial class LoginWindow : MetroWindow
    {
        private DatabaseConnection connection;
        private BackgroundWorker worker;
        private LoginRole usuario;
        private string error;
        private int check;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void buttonEntrar_Click(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            usuario.Username = loginBox.Text;
            usuario.Password = senhaBox.Password;

            buttonEntrar.Visibility = Visibility.Hidden;
            progressRing.Visibility = Visibility.Visible;

            Mouse.OverrideCursor = Cursors.Wait;

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((string.IsNullOrEmpty(usuario.Username)) && (string.IsNullOrEmpty(usuario.Password)))
            {
                error = "Digite o usuário e a senha cadastrados.";
                return;
                //throw new NotSupportedException("Digite o usuário e a senha cadastrados.");
            }

            if (string.IsNullOrEmpty(usuario.Username))
            {
                error = "Digite o usuário cadastrado.";
                return;
                //throw new Exception("Digite o usuário cadastrado.");
            }

            if (string.IsNullOrEmpty(usuario.Password))
            {
                error = "Digite a senha cadastrada.";
                return;
                //throw new Exception("Digite a senha cadastrada.");
            }

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();
            }
            catch (SqlException ex)
            {
                error = ex.Message;
                connection.Close();
                return;
            }

            System.Threading.Thread.Sleep(3000);

            error = null;
            string sql = "SELECT IS_MEMBER('supervisor');";
            check = connection.Return(sql);
            
            connection.Close();
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            if (error == null)
            {
                nextWindow(check);
            }
            else
            {
                buttonEntrar.Visibility = Visibility.Visible;
                progressRing.Visibility = Visibility.Hidden;
                labelNull.Content = error;
            }
        }

        private void nextWindow(int check_supervisor)
        {
            if (check == 1)
            {
                Supervisor.Supervisor supervisor = new Supervisor.Supervisor();
                supervisor.Show();
            }
            else
            {
                Intern.Intern intern = new Intern.Intern();
                intern.Show();
            }

            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Common.About sobre = new Common.About();
            sobre.ShowDialog();
        }
    }
}
