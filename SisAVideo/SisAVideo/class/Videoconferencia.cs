using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SisAVideo
{
    class Videoconferencia
    {
        private LoginRole usuario;
        private DatabaseConnection connection;
        private string sql_rowcount = "SELECT @@ROWCOUNT;";
        private int colunas;

        private string titulo;
        private int codigo;
        private string data_ativacao;
        private string horario_inicio;
        private string horario_fim;
        private string solicitante;
        private string secretaria;
        private string orgao;
        private string streaming;
        private string estudio;
        private string target;

        protected static Regex _link = new Regex(@"^http:\/\/media.escolasdegoverno.sp.gov.br\/..*$");
        protected static Regex _data = new Regex(@"^((0?[1-9]|[12]\d)\/(0?[1-9]|1[0-2])|30\/(0?[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$");
        
        public string Titulo
        {
            get { return titulo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    titulo = value; 
            }
        }

        public int Codigo
        {
            get { return codigo; }
            set
            {
                colunas = 0;

                try
                {
                    usuario = new LoginRole();
                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();
                    string sql_unique = "SELECT codigo FROM vc.videoconferencia "
                                      + "WHERE codigo = '" + value + "' ;";
                    connection.SQL(sql_unique);
                    colunas = connection.Return(sql_rowcount);
                    connection.Close();
                }
                catch
                {
                    throw new NotSupportedException();
                }

                if ((string.IsNullOrEmpty(value.ToString())) || (colunas > 0))
                    throw new NotSupportedException();
                else
                    codigo = value;
            }
        }

        public string Data_ativacao
        {
            get { return data_ativacao; }
            set 
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    data_ativacao = value; 
            }
        }

        public string Horario_inicio
        {
            get { return horario_inicio; }
            set 
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_inicio = value;
                else
                    throw new NotSupportedException();
            }
        }

        public string Horario_fim
        {
            get { return horario_fim; }
            set 
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_fim = value; 
                else
                    throw new NotSupportedException();
            }
        }

        public string Solicitante
        {
            get { return solicitante; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    solicitante = value;
            }
        }

        public string Secretaria
        {
            get { return secretaria; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    secretaria = value;
            }
        }

        public string Orgao
        {
            get { return orgao; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    orgao = value;
            }
        }

        public string Streaming
        {
            get { return streaming; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    streaming = null;
                else
                {
                    if (!_link.IsMatch(value))
                        throw new NotSupportedException();
                    else
                        streaming = value;
                }
            }
        }

        public string Estudio
        {
            get { return estudio; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    estudio = value;
            }
        }

        public string Target
        {
            get { return target; }
            set { target = value; }
        }
    }
}
