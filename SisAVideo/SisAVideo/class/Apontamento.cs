using System;

namespace SisAVideo
{
    class Apontamento
    {
        private TimeSpan agora;
        private TimeSpan manha_inicio_entrada;
        private TimeSpan manha_final_entrada;
        private TimeSpan manha_inicio_saida;
        private TimeSpan manha_final_saida;
        private TimeSpan tarde_inicio_entrada;
        private TimeSpan tarde_final_entrada;
        private TimeSpan tarde_inicio_saida;
        private TimeSpan tarde_final_saida;

        public TimeSpan Agora
        {
            get { return agora; }
            set { agora = value; }
        }

        public TimeSpan Manha_inicio_entrada
        {
            get { return manha_inicio_entrada; }
            set { manha_inicio_entrada = value; }
        }

        public TimeSpan Manha_final_entrada
        {
            get { return manha_final_entrada; }
            set { manha_final_entrada = value; }
        }

        public TimeSpan Manha_inicio_saida
        {
            get { return manha_inicio_saida; }
            set { manha_inicio_saida = value; }
        }

        public TimeSpan Manha_final_saida
        {
            get { return manha_final_saida; }
            set { manha_final_saida = value; }
        }

        public TimeSpan Tarde_inicio_entrada
        {
            get { return tarde_inicio_entrada; }
            set { tarde_inicio_entrada = value; }
        }

        public TimeSpan Tarde_final_entrada
        {
            get { return tarde_final_entrada; }
            set { tarde_final_entrada = value; }
        }

        public TimeSpan Tarde_inicio_saida
        {
            get { return tarde_inicio_saida; }
            set { tarde_inicio_saida = value; }
        }

        public TimeSpan Tarde_final_saida
        {
            get { return tarde_final_saida; }
            set { tarde_final_saida = value; }
        }


    }
}
