using System;
namespace SisAVideo
{
    class Telefone : Funcionario
    {
        private string telefone;

        public string Tel
        {
            get { return telefone; }
            set
            {
                if ((_tel11.IsMatch(value)) || (_tel.IsMatch(value)))
                    telefone = value;
                else
                    throw new NotSupportedException();
            }
        }
    }
}
