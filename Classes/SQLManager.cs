using System;
using System.Data;
using System.Data.SqlClient;

namespace Bank_Credit_Manager
{
    class SQLManager : ISQLManager
    {
        ///<summary>
        ///Строка соединения
        ///</summary>
        public string ConnectionString()
        {
            return "Data Source=localhost;Initial catalog=Sorbon;Integrated Security=True";
        }

        ///<summary>
        ///Вводит новые данные в таблицу
        ///</summary>
        public void InsertData(string tableName, string columns, string values)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(this.ConnectionString());
                sqlConn.Open();
                if (!this.isConnected(sqlConn)) return;
                SqlCommand sqlCmd = new SqlCommand($"insert into [dbo].[{tableName}] ({columns}) values({values})", sqlConn);  
                sqlCmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }

        }

        ///<summary>
        ///Возвращает True если соединен с базой данных, иначе Else
        ///</summary>
        public bool isConnected(SqlConnection sqlConn)
        {
            return sqlConn.State == ConnectionState.Open;
        }

        ///<summary>
        ///Обновление данных
        ///</summary>
        public void UpdateData(string tableName, string query, string cond)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(this.ConnectionString());
                sqlConn.Open();
                if (!this.isConnected(sqlConn)) return;
                SqlCommand sqlCmd = new SqlCommand($"update [dbo].[{tableName}] set {query} where {cond}", sqlConn);  
                sqlCmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
