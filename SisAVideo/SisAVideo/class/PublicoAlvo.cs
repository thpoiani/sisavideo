using System;

namespace SisAVideo
{
    class PublicoAlvo
    {
        private string publicoalvo;
        
        public string Publicoalvo
        {
            get { return publicoalvo; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    publicoalvo = value; 
            }
        }
    }

 
}
