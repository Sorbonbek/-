using System;
using System.Data;
using System.Data.SqlClient;

namespace Bank_Credit_Manager
{
    public class Authentication : IAuthentication
    {
        private string _username = string.Empty;
        private string _userpassword = string.Empty;
        public string DateOfBirth = string.Empty;
        public string HomePath = string.Empty;
        public string Seria = string.Empty;
        public int LoginUser = 0;
        private readonly SQLManager _sqlManage = new SQLManager();
        public Authentication(string name, string password)
        {
            _username = name;
            _userpassword = password;
        }

        ///<summary>
        ///Регистрация аккаунта
        ///</summary>
        public bool RegistrateAccount(string tableName)
        {
            if(this.isPreviouslyCreated(false))
                return false;
            else
            {
                try
                {
                    SQLManager sqlManager = new SQLManager();
                    if(tableName == "users_list_table")
                        sqlManager.InsertData("users_list_table", "_name, _login, _password, _date_of_birth, _home_path, _seria", $"'{_username}', {LoginUser}, '{_userpassword}', '{DateOfBirth}', '{HomePath}', '{Seria}'");
                    else
                        sqlManager.InsertData($"{tableName}", "_name, _password", "'{username}', '{userpassword}'");
                    return true;
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message);
                    return false;
                }
            }
        }

        ///<summary>
        ///Возвращает True если пользователь с заданым логином существует, иначе Else
        ///</summary>
        public bool isPreviouslyCreated(bool admin)
        {
            string query = string.Empty;
            if(admin)
                query = $"select _name from [dbo].[admin_list_table] where _name='{_username}'";
            else
                query = $"select _name from [dbo].[users_list_table] where _name='{_username}'";
            bool created = false;
            int counted = 0;
            SqlConnection sqlConn = new SqlConnection(_sqlManage.ConnectionString());
            sqlConn.Open();
            if(sqlConn.State == ConnectionState.Open)
            {
                SqlCommand sqlCmd = new SqlCommand(query, sqlConn);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while(reader.Read())
                {
                    counted++;
                }
                created = counted > 0;
                reader.Close();
                sqlConn.Close();
            }
            return created;
        }

        ///<summary>
        ///Авторизация
        ///</summary>
        public bool Login(string tableName)
        {
            string query = string.Empty;
            if(tableName == "users_list_table")
                query = $"select _login, _password from [dbo].[users_list_table] where (_login={LoginUser} and _password='{_userpassword}')";
            else if(tableName == "admin_list_table")
                query = $"select _name, _password from [dbo].[admin_list_table] where (_name='{_username}' and _password='{_userpassword}')";
            
            bool logged = true;
            int counted = 0;
            SqlConnection sqlConn = new SqlConnection(_sqlManage.ConnectionString());
            sqlConn.Open();
            if (sqlConn.State != ConnectionState.Open) return logged;
            SqlCommand sqlCmd = new SqlCommand(query, sqlConn);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while(reader.Read())
            {
                counted++;
            }
            logged = counted > 0;
            reader.Close();
            sqlConn.Close();
            return logged;
        }
    }
}