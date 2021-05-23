using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SisAVideo
{
    class DatabaseConnection
    {
        private string connectionString;
        private SqlConnection conexao;
        private SqlTransaction transaction;
        private SqlCommand sql;
        private SqlDataAdapter ponte; // ponte entre DataSet e SQLServer
        private DataSet dataset;

        public DatabaseConnection(string user, string pass)
        {
            connectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            connectionString += "User Id=" + user + ";Password=" + pass + ";";
            conexao = new SqlConnection(connectionString);
            transaction = null;
        }

        public SqlConnection Open()
        {
            if ((conexao.State == ConnectionState.Closed) || (conexao.State == ConnectionState.Broken))
                conexao.Open();
            return conexao;
        }
        
        public SqlConnection Close()
        {
            if (conexao.State == ConnectionState.Open)
                conexao.Close();
            return conexao;
        }

        public void SQL(string comando)
        {
            transaction = conexao.BeginTransaction();
            try
            {
                sql = new SqlCommand(comando, conexao, transaction);
                sql.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        public int Return(string comando)
        {
            int query = 0;
            transaction = conexao.BeginTransaction();
            try
            {
                sql = new SqlCommand(comando, conexao, transaction);
                query = (int)sql.ExecuteScalar();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return query;
        }

        public string ReturnString(string comando)
        {
            string query = null;
            transaction = conexao.BeginTransaction();
            try
            {
                sql = new SqlCommand(comando, conexao, transaction);
                query = sql.ExecuteScalar().ToString();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return query;
        }

        public DataSet View(string Binding)
        {
            ponte = new SqlDataAdapter(sql);
            dataset = new DataSet();
            dataset.Reset();
            ponte.Fill(dataset, Binding); // adiciona ou atualiza os registros
            return dataset;
        }
    }
}