using System;

namespace SisAVideo
{
    class Estagiario : Funcionario
    {

        private string nome_funcionario;
        private string nome_polo;
        private string instituicao;
        private string curso;
        private string inicio_curso;
        private string fim_curso;
        private string data_contrato;
        private string horario_entrada;
        private string horario_saida;

        public string Nome_funcionario
        {
            get { return nome_funcionario; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    nome_funcionario = value; 
            }
        }

        public string Nome_polo
        {
            get { return nome_polo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    nome_polo = value;
            }
        }
        
        public string Instituicao
        {
            get { return instituicao; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    instituicao = value; 
            }
        }

        public string Curso
        {
            get { return curso; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NotSupportedException();
                else
                    curso = value;
            }
        }

        public string Inicio_curso
        {
            get { return inicio_curso; }
            set
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    inicio_curso = value;
            }
        }

        public string Fim_curso
        {
            get { return fim_curso; }
            set
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    fim_curso = value;
            }
        }

        public string Data_contrato
        {
            get { return data_contrato; }
            set
            {
                if (!_data.IsMatch(value))
                    throw new NotSupportedException();
                else
                    data_contrato = value;

            }
        }

        public string Horario_entrada
        {
            get { return horario_entrada; }
            set
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_entrada = value;
                else
                    throw new NotSupportedException();
            }
        }

        public string Horario_saida
        {
            get { return horario_saida; }
            set
            {
                DateTime datetime;
                if (DateTime.TryParse(value, out datetime))
                    horario_saida = value;
                else
                    throw new NotSupportedException();
            }
        }
    }
}
