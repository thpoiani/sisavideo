using System;
namespace SisAVideo
{
    class Login : Funcionario
    {
        private string username;
        private string password;
        static private string permissao;

        public string Username
        {
            get { return username; }
            set
            {
                colunas = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();

                    // query que retorna os usernames do database
                    string sql_unique = "SELECT u.name AS users FROM sys.server_principals u "
                                      + "INNER JOIN master.dbo.sysdatabases b "
                                      + "ON u.create_date > b.crdate "
                                      + "WHERE b.name = 'sisavideo' "
                                      + "AND u.name = '" + value + "' ;";
                    connection.SQL(sql_unique);
                    colunas = connection.Return(sql_rowcount);

                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((string.IsNullOrEmpty(value)) || (colunas > 0))
                    throw new NotSupportedException();

                username = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if ((string.IsNullOrEmpty(value)) || (username == value))
                    throw new NotSupportedException();

                password = value; 
            }
        }

        public string Permissao
        {
            get { return permissao; }
            set { permissao = value; }
        }
    }
}
