using System.Data;
using System.Data.SqlClient;

namespace Bank_Credit_Manager
{
    public class ClientApplication : IClientApplication
    {
        private readonly string _login;
        public ClientApplication(string clientLogin)
        {
            _login = clientLogin;
        }

        ///<summary>
        ///Регистрация заявки пользователя для получения кредита
        ///</summary>
        public void CreateApplication(string gender, string isMarried, int age, string nation, int creditSumm, string creditAim, int creditTerm)
        {
            SQLManager sqlManager = new SQLManager();
            sqlManager.InsertData("users_application", "_login, _user_gender, _user_age, _married, _nationality, _credit_summ_from_general_revenue, _credit_history, _arrearage_in_credit_history, _credit_aim, _credit_term, _status, _balls, _results, _is_payed",
            $"{_login}, '{gender}', {age}, '{isMarried}', '{nation}', {creditSumm}, {this.CreditsCount()}, {this.CreditArrearage()}, '{creditAim}', {creditTerm}, 'NONE', 0, 0, 0");
        }

        ///<summary>
        ///Возвращает кол-во не оплаченных кредитов
        ///</summary>
        public int CreditArrearage()
        {
            int results = 0;
            SQLManager sqlManager = new SQLManager();
            SqlConnection sqlConn = new SqlConnection(sqlManager.ConnectionString());
            sqlConn.Open();
            if (sqlConn.State != ConnectionState.Open) return results;
            SqlCommand sqlCmd = new SqlCommand("select _summ from [dbo].[payment_list] where _summ != 0", sqlConn);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                results++;
            }
            reader.Close();
            sqlConn.Close();
            return results;
        }

        ///<summary>
        ///Возвращает кол-во кредитов
        ///</summary>
        public int CreditsCount()
        {
            int results = 0;
            SQLManager sqlManager = new SQLManager();
            SqlConnection sqlConn = new SqlConnection(sqlManager.ConnectionString());
            sqlConn.Open();
            if (sqlConn.State != ConnectionState.Open) return results;
            SqlCommand sqlCmd = new SqlCommand("select _status from [dbo].[users_application] where _status = 'OK'", sqlConn);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                results++;
            }
            reader.Close();
            sqlConn.Close();
            return results;
        }

        ///<summary>
        ///Проверка заявки клиента на получение кредита, True если одобрен, иначе Else
        ///</summary>
        public bool AcceptedToCredit()
        {
            int balls = 0;
            SQLManager sqlManager = new SQLManager();
            SqlConnection sqlConn = new SqlConnection(sqlManager.ConnectionString());
            sqlConn.Open();
            if (sqlConn.State == ConnectionState.Open)
            {
                SqlCommand sqlCmd = new SqlCommand($"select _user_gender, _user_age, _married, _nationality, _credit_summ_from_general_revenue, _credit_history, _arrearage_in_credit_history, _credit_aim, _credit_term from [Faridun].[dbo].[users_application] where _login={_login}", sqlConn);
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                string userGender = string.Empty;
                int userAge = 0;
                string married = string.Empty;
                string nationality = string.Empty;
                int creditSummFromGeneralRevenue = 0;
                int creditHistory = 0;
                int arrearageInCreditHistory = 0;
                string creditAim = string.Empty;
                int creditTerm = 0;
                while (sqlReader.Read())
                {
                    userGender = sqlReader.GetValue(0).ToString()?.Trim();
                    userAge = int.Parse(sqlReader.GetValue(1).ToString() ?? string.Empty);
                    married = sqlReader.GetValue(2).ToString()?.Trim();
                    nationality = sqlReader.GetValue(3).ToString()?.Trim();
                    creditSummFromGeneralRevenue = int.Parse(sqlReader.GetValue(4).ToString() ?? string.Empty);
                    creditHistory = int.Parse(sqlReader.GetValue(5).ToString() ?? string.Empty);
                    arrearageInCreditHistory = int.Parse(sqlReader.GetValue(6).ToString() ?? string.Empty);
                    creditAim = sqlReader.GetValue(7).ToString()?.Trim();
                    creditTerm = int.Parse(sqlReader.GetValue(8).ToString() ?? string.Empty);
                }
                sqlReader.Close();
                sqlConn.Close();
                switch (userGender)
                {
                    case "муж":
                        balls++;
                        break;
                    case "жен":
                        balls += 2;
                        break;
                }

                switch (married)
                {
                    case "холост":
                        balls++;
                        break;
                    case "семеянин":
                        balls += 2;
                        break;
                    case "вразводе":
                        balls++;
                        break;
                    case "вдовец/вдова":
                        balls += 2;
                        break;
                }


                if (userAge < 25)
                    balls += 0;
                else if (userAge >= 25 || userAge <= 35)
                    balls++;
                else if (userAge >= 36 || userAge <= 62)
                    balls += 2;
                else if (userAge >= 63)
                    balls++;

                switch (nationality)
                {
                    case "Таджикистан":
                        balls++;
                        break;
                    case "Зарубеж":
                        balls += 0;
                        break;
                }

                if (creditSummFromGeneralRevenue < 80)
                    balls += 4;
                else if (creditSummFromGeneralRevenue >= 80 || creditSummFromGeneralRevenue <= 150)
                    balls += 3;
                else if (creditSummFromGeneralRevenue > 150 || creditSummFromGeneralRevenue <= 250)
                    balls += 2;
                else if (creditSummFromGeneralRevenue > 250)
                    balls += 1;

                if (creditHistory > 3)
                    balls += 2;
                else if (creditHistory == 1 || creditHistory == 2)
                    balls++;
                else if (creditHistory == 0)
                    balls -= 1;

                if (arrearageInCreditHistory > 7)
                    balls -= 3;
                else if (arrearageInCreditHistory >= 5 || arrearageInCreditHistory <= 7)
                    balls -= 2;
                else if (arrearageInCreditHistory == 4)
                    balls -= 1;
                else if (arrearageInCreditHistory < 3)
                    balls += 0;


                if (creditAim == "бытовая техника")
                    balls += 2;
                else if (creditAim == "ремонт")
                    balls++;
                else if (creditAim == "телефон")
                    balls += 0;
                else if (creditAim == "прочее")
                    balls -= 1;

                if (creditTerm > 12 || creditTerm <= 12)
                    balls++;
            }

            if (balls > 11)
            {
                sqlManager.UpdateData("users_application", $"_balls={balls}, _status='OK'", $"_login={_login}");
                return true;
            }
            else
            {
                sqlManager.UpdateData("users_application", $"_balls={balls}, _status='DISOK'", $"_login={_login}");
                return false;
            }
        }
    }
}