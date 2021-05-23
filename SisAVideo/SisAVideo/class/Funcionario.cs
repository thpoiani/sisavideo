using System;
using System.Text.RegularExpressions;

namespace SisAVideo
{
    class Funcionario
    {
        public LoginRole usuario;
        public DatabaseConnection connection;
        public string sql_rowcount = "SELECT @@ROWCOUNT;";
        public int colunas;

        protected static Regex _cpf = new Regex(@"^\d{3}\.?\d{3}\.?\d{3}\-?\d{2}$");
        protected static Regex _rg = new Regex(@"^\d{2}\.?\d{3}\.?\d{3}\-?[Xx0-9]{1}$");
        protected static Regex _email = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
        protected static Regex _data = new Regex(@"^((0?[1-9]|[12]\d)\/(0?[1-9]|1[0-2])|30\/(0?[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$");
        protected static Regex _num = new Regex(@"^\d+$");
        protected static Regex _tel11 = new Regex(@"^[(][1][1][)]\s[9]\s\d{4}-\d{4}$");
        protected static Regex _tel = new Regex(@"^[(]\d{1}[2-9]{1}[)]\s[_]\s\d{4}-\d{4}$");
        protected static Regex _cep = new Regex(@"^[0-9]{5}-[0-9]{3}$");

        private int id;
        private string nome;
        private string cpf;
        private string rg;
        private string email;
        private char sexo;
        private string data_nasc;
        private string logradouro;
        private int numero;
        private string complemento;
        private string cep;
        private string cidade;
        private string cargo;
        private string data_admissao;
        private string data_demissao;

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
                    string sql_unique = "SELECT nome FROM rh.funcionario "
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


        public string Cpf
        {
            get { return cpf; }
            set
            {
                colunas = 0;

                try
                {
                    connection.Open();
                    string sql_unique = "SELECT cpf FROM rh.funcionario "
                                      + "WHERE cpf = '" + value + "' ;";
                    connection.SQL(sql_unique);
                    colunas = connection.Return(sql_rowcount);
                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((!_cpf.IsMatch(value)) || (colunas > 0))
                    throw new NotSupportedException();
                else
                    cpf = value;
            }
        }


        public string Rg
        {
            get { return rg; }
            set
            {
                colunas = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();
                    string sql_unique = "SELECT rg FROM rh.funcionario "
                                      + "WHERE rg = '" + value + "' ;";
                    connection.SQL(sql_unique);
                    colunas = connection.Return(sql_rowcount);
                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((!_rg.IsMatch(value)) || (colunas > 0))
                    throw new NotSupportedException();
                else
                    rg = value;
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
                    string sql_unique = "SELECT email FROM rh.funcionario "
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


        public char Sexo
        {
            get { return sexo; }
            set
            {
                char C1 = 'M';
                char c1 = 'm';
                char C2 = 'F';
                char c2 = 'f';

                if (value.Equals(C1) || value.Equals(c1)
                 || value.Equals(C2) || value.Equals(c2))
                    sexo = value;
                else
                    throw new NotSupportedException();
            }
        }


        public string Data_nasc
        {
            get { return data_nasc; }
            set
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    data_nasc = value;                    
            }
        }


        public string Logradouro
        {
            get { return logradouro; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    logradouro = value;
            }
        }


        public int Numero
        {
            get { return numero; }
            set
            {
                if (!_num.IsMatch(value.ToString()))
                    throw new NotSupportedException();
                else
                    numero = value;
            }
        }


        public string Complemento
        {
            get { return complemento; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    complemento = null;
                else
                    complemento = value;
            }
        }


        public string Cep
        {
            get { return cep; }
            set
            {
                if (!_cep.IsMatch(value))
                    throw new NotSupportedException();
                else
                    cep = value;                    
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


        public string Cargo
        {
            get { return cargo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    cargo = null;
                else
                    cargo = value;
            }
        }


        public string Data_admissao
        {
            get { return data_admissao; }
            set
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    data_admissao = value;                    
            }
        }


        public string Data_demissao
        {
            get { return data_demissao; }
            set
            {
                if ((_data.IsMatch(value)) || (string.IsNullOrEmpty(value)))
                    data_demissao = value;
                else
                    throw new NotSupportedException();
            }
        }
    }
}
