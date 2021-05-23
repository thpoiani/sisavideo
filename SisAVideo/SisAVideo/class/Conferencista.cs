using System;
using System.Text.RegularExpressions;

namespace SisAVideo
{
    class Conferencista
    {
        protected static Regex _cpf = new Regex(@"^\d{3}\.?\d{3}\.?\d{3}\-?\d{2}$");
        
        private string cpf;
        private string nome;

        public string Cpf
        {
            get { return cpf; }
            set 
            {
                if (!_cpf.IsMatch(value))
                    throw new NotSupportedException();
                else
                    cpf = value;             
            }
        }

        public string Nome
        {
            get { return nome; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    nome = value; 
            }
        }
    }
}
