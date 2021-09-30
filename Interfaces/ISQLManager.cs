using System.Data.SqlClient;

namespace Bank_Credit_Manager
{
    public interface ISQLManager
    {
        string ConnectionString();
        void InsertData(string _tableName, string _columns, string _values);
        void UpdateData(string _tableName, string _query, string _cond);
        bool isConnected(SqlConnection _sqlConn);
    }
}