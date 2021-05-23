using System;
using System.Text.RegularExpressions;
namespace SisAVideo
{
    class Polo
    {
        private LoginRole usuario;
        private DatabaseConnection connection;
        private string sql_rowcount = "SELECT @@ROWCOUNT;";
        private int colunas;

        protected static Regex _num = new Regex(@"^\d+$");
        protected static Regex _email = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
        protected static Regex _tel11 = new Regex(@"^[(][1][1][)]\s[9]\s\d{4}-\d{4}$");
        protected static Regex _tel = new Regex(@"^[(]\d{1}[2-9]{1}[)]\s[_]\s\d{4}-\d{4}$");

        private int id;
        private string nome;
        private string endereco;
        private string cidade;
        private int capacidade;
        private string horario_abertura;
        private string horario_fechamento;
        private string email;
        private string senha;
        private string celular;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public string Nome
        {
            get { return nome; }
            set 
            {
                colunas = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();
                    string sql_unique = "SELECT nome FROM vc.polo "
                                      + "WHERE nome = '" + value + "' ;";
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
                else
                    nome = value; 
            }
        }

        public string Endereco
        {
            get { return endereco; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    endereco = value; 
            }
        }
        
        public string Cidade
        {
            get { return cidade; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    cidade = value; 
            }
        }
        
        public int Capacidade
        {
            get { return capacidade; }
            set 
            {
                if (!_num.IsMatch(value.ToString()))
                    throw new NotSupportedException();
                else
                    capacidade = value; 
            }
        }
        
        public string Horario_abertura
        {
            get { return horario_abertura; }
            set 
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_abertura = value;
                else
                    throw new NotSupportedException();
            }
        }

        public string Horario_fechamento
        {
            get { return horario_fechamento; }
            set 
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_fechamento = value; 
                else
                    throw new NotSupportedException();
            }
        }

        public string Email
        {
            get { return email; }
            set 
            {
                colunas = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();
                    string sql_unique = "SELECT email FROM vc.polo "
                                      + "WHERE email = '" + value + "' ;";
                    connection.SQL(sql_unique);
                    colunas = connection.Return(sql_rowcount);
                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((!_email.IsMatch(value)) || (colunas > 0))
                    throw new NotSupportedException();
                else
                    email = value; 
            }
        }
        
        public string Senha
        {
            get { return senha; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    senha = value; 
            }
        }

        public string Celular
        {
            get { return celular; }
            set 
            {
                if ((_tel.IsMatch(value)) || (_tel11.IsMatch(value)))
                    celular = value;
                else
                    throw new NotSupportedException();
            }
        }   
    }
}
