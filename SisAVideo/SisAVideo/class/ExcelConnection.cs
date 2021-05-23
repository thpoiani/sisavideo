using System.Data;
using System.Data.OleDb;

namespace SisAVideo
{

    class ExcelConnection
    {
        private string connectionString;
        private OleDbConnection conexao;
        private OleDbCommand sql;
        private OleDbTransaction transaction;
        
        public ExcelConnection(string planilha)
        {
            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + planilha + ";Extended Properties='Excel 8.0;HDR=No;'";
            conexao = new OleDbConnection(connectionString);
            transaction = null;
        }

        public OleDbConnection Open()
        {
            if ((conexao.State == ConnectionState.Closed) || (conexao.State == ConnectionState.Broken))
            {
                conexao.Open();
            }
            return conexao;
        }

        public OleDbConnection Close()
        {
            if (conexao.State == ConnectionState.Open)
            {
                conexao.Close();
            }
            return conexao;
        }

        public void SQL(string comando)
        {
            transaction = conexao.BeginTransaction();
            try
            {
                sql = new OleDbCommand(comando, conexao, transaction);
                sql.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

    }
}
