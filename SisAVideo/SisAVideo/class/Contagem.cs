using System;
using System.Text.RegularExpressions;

namespace SisAVideo
{
    class Contagem
    {
        protected static Regex _num = new Regex(@"^\d+$");

        private int numero;

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
    }
}
