using System;
using System.Text.RegularExpressions;
namespace SisAVideo
{
    class PoloAtivo
    {
        public LoginRole usuario;
        public DatabaseConnection connection;
        public int id;

        protected static Regex _num = new Regex(@"^\d+$");
        
        private string codigo_polo;
        private int contagem;
        private string horario_contagem;

        public string Codigo_polo
        {
            get { return codigo_polo; }
            set
            {
                id = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();
                    
                    string sql_unique = "SELECT id FROM vc.polo "
                                      + "WHERE nome = '" + value + "' ;";
                    id = connection.Return(sql_unique);
                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((string.IsNullOrEmpty(value)) || (id == 0))
                    throw new NotSupportedException();
                else
                    codigo_polo = id.ToString();
            }

        }

        public int Contagem
        {
            get { return contagem; }
            set 
            {
                if (!_num.IsMatch(value.ToString()))
                    throw new NotSupportedException();
                else
                    contagem = value; 
            }
        }

        public string Horario_contagem
        {
            get { return horario_contagem; }
            set 
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_contagem = value; 
                else
                    throw new NotSupportedException();
            }

        }
    }
}
