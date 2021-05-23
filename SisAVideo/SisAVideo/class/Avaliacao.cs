using System;
using System.Text.RegularExpressions;
namespace SisAVideo
{
    class Avaliacao
    {
        protected static Regex _data = new Regex(@"^((0?[1-9]|[12]\d)\/(0?[1-9]|1[0-2])|30\/(0?[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$");
        protected static Regex _num = new Regex(@"^\d+$");

        private string titulo;
        private int codigo;
        private string estagiario;
        private string data;
        private int totalParticipantes;
        private int totalRespostas;
        private string participantes;
        private int lbl11;
        private int lbl12;
        private int lbl13;
        private int lbl14;
        private int lbl21;
        private int lbl22;
        private int lbl23;
        private int lbl311;
        private int lbl312;
        private int lbl321;
        private int lbl322;
        private int lbl331;
	    private string obs;

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
                if (string.IsNullOrEmpty(value.ToString()))
                    throw new NotSupportedException();
                else
                    codigo = value;
            }
        }

        public string Estagiario
        {
            get { return estagiario; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    estagiario = value;
            }
        }

        public string Data
        {
            get { return data; }
            set 
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    data = value; 
            }
        }

        public int TotalParticipantes
        {
            get { return totalParticipantes; }
            set 
            {
                if (!_num.IsMatch(value.ToString()))
                    throw new NotSupportedException();
                else
                    totalParticipantes = value; 
            }
        }

        public int TotalRespostas
        {
            get { return totalRespostas; }
            set
            {
                if (!_num.IsMatch(value.ToString()))
                    throw new NotSupportedException();
                else
                    totalRespostas = value;
            }
        }

        
        public string Participantes
        {
            get { return participantes; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    participantes = value; 
            }
        }

        public int Lbl11
        {
            get { return lbl11; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl11 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl12
        {
            get { return lbl12; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl12 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl13
        {
            get { return lbl13; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl13 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl14
        {
            get { return lbl14; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl14 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl21
        {
            get { return lbl21; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl21 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl22
        {
            get { return lbl22; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl22 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl23
        {
            get { return lbl23; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl23 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl311
        {
            get { return lbl311; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl311 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl312
        {
            get { return lbl312; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl312 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl321
        {
            get { return lbl321; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl321 = value;
                else
                    throw new NotSupportedException();
            }
        }


        public int Lbl322
        {
            get { return lbl322; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl322 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public int Lbl331
        {
            get { return lbl331; }
            set
            {
                if ((_num.IsMatch(value.ToString())) && (value == TotalRespostas))
                    lbl331 = value;
                else
                    throw new NotSupportedException();
            }
        }

        public string Obs
        {
            get { return obs; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    obs = value; 
            }
        }
    }
}
