using System;
using System.Data;
using System.Data.SqlClient;

namespace Bank_Credit_Manager
{
    public class UserInterface : IUserInterface
    {
        ///<summary>
        ///Возвращает строку с символами которые ввел пользователь
        ///</summary>
        public string Input()
        {
            return Console.ReadLine();
        }

        ///<summary>
        ///Выводит параметр _text в консоль и возвращает строку с символами которые ввел пользователь
        ///</summary>
        public string Input(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        ///<summary>
        ///Вывод текста в консоль
        ///</summary>
        public void Output(string text)
        {
            Console.WriteLine(text);
        }

        ///<summary>
        ///Авторизация пользователя
        ///</summary>
        public void LoginOutput()
        {
            bool isAdmin = false;
            string login = string.Empty;
            while (login != "/Назад/")
            {
                login = Input("Логин (Номер телефона): ");
                string password = string.Empty;
                if (login == "/admin/")
                    isAdmin = true;
                else
                    password = Input("Пароль: ");

                if (isAdmin)
                {
                    this.Output("Вы вошли в панель администратора!");
                    string adminLogin = Input("Введите имя: ");
                    string adminPassword = Input("Введите пароль: ");
                    Authentication auth = new Authentication(adminLogin, adminPassword);
                    bool isLogged = auth.Login("admin_list_table");
                    if (isLogged)
                    {
                        this.Output("Приветствую вас: " + adminLogin);
                        this.AdminOutput();
                    }
                    else
                    {
                        this.Output("Не правильный логин или пароль!");
                    }
                }
                else
                {
                    Authentication auth = new Authentication(login, password);
                    auth.LoginUser = int.Parse(login);
                    bool isLogged = auth.Login("users_list_table");
                    if (isLogged)
                    {
                        SQLManager sqlManger = new SQLManager();
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State != ConnectionState.Open) continue;
                        SqlCommand sqlCmd = new SqlCommand($"select _name from users_list_table where _login={login}", sqlConn);
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        string loggedUserName = string.Empty;
                        while (reader.Read())
                        {
                            loggedUserName = reader.GetValue(0).ToString()?.Trim();
                        }
                        this.Output("Приветствую вас: " + loggedUserName);
                        this.UserOutput(login);
                        reader.Close();
                        sqlConn.Close();
                    }
                    else
                    {
                        this.Output("Не правильный логин или пароль!");
                    }
                }
            }
        }

        ///<summary>
        ///Панель администратора
        ///</summary>
        public void AdminOutput()
        {
            this.Output("Панель администратора:\t1.Просмотр заявок\t2.Просмотр клиентов\t0.Выход");
            string cmd = string.Empty;
            while (cmd != "0")
            {
                cmd = this.Input("Выберите действие(1,2,0): ");
                switch (cmd)
                {
                    case "1":
                    {
                        string name = this.Input("Введите логин(номер телефона) пользователя: ");
                        SQLManager sqlManger = new SQLManager();
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select _user_gender, _user_age, _married, _nationality, _credit_summ_from_general_revenue, _credit_aim, _credit_term, _results from [dbo].[users_application] where _login={name}", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                this.Output("Пол: " + reader.GetValue(0).ToString()?.Trim());
                                this.Output("Возраст: " + reader.GetValue(1).ToString()?.Trim());
                                this.Output("Семейное положение: " + reader.GetValue(2).ToString()?.Trim());
                                this.Output("Гражданство: " + reader.GetValue(3).ToString()?.Trim());
                                this.Output("Cумма кредита от общего дохода: " + reader.GetValue(4).ToString()?.Trim());
                                this.Output("Цель кредита: " + reader.GetValue(5).ToString()?.Trim());
                                this.Output("Срок кредита: " + reader.GetValue(6).ToString()?.Trim());
                                this.Output("Результат: " + reader.GetValue(7).ToString()?.Trim());
                            }

                            reader.Close();
                            sqlConn.Close();
                        }

                        break;
                    }
                    case "2":
                    {
                        this.Output("Список всех клиентов: ");
                        SQLManager sqlManger = new SQLManager();
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select * from [dbo].[users_list_table]", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                this.Output("ID: " + reader.GetValue(0));
                                this.Output("Имя: " + reader.GetValue(1));
                                this.Output("Логин (номер телнфона): " + reader.GetValue(2));
                                this.Output("Пароль: " + reader.GetValue(3));
                                this.Output("ДР: " + reader.GetValue(4));
                                this.Output("Прописка: " + reader.GetValue(5));
                                this.Output("Серия паспорта: " + reader.GetValue(6));
                            }

                            reader.Close();
                            sqlConn.Close();
                        }

                        break;
                    }
                }
            }
        }

        ///<summary>
        ///Личный кабинет пользователя
        ///</summary>
        public void UserOutput(string _name)
        {
            this.Output("Панель пользователя(клиента):\t1.Просмотерть заявок\t2.Остаток кредитов\t3.Детали кредита в виде графика погашения\t4.Подать заявку\t0.Выход");
            string cmd = string.Empty;
            while (cmd != "0")
            {
                cmd = this.Input("Выберите действие(1,2,3,0): ");
                SQLManager sqlManger = new SQLManager();
                switch (cmd)
                {
                    case "1":
                    {
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select _user_gender, _user_age, _married, _nationality, _credit_summ_from_general_revenue, _credit_aim, _credit_term, _results from [Faridun].[dbo].[users_application] where _login={_name}", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                this.Output("Пол: " + reader.GetValue(0).ToString()?.Trim());
                                this.Output("Возраст: " + reader.GetValue(1).ToString()?.Trim());
                                this.Output("Семейное положение: " + reader.GetValue(2).ToString()?.Trim());
                                this.Output("Гражданство: " + reader.GetValue(3).ToString()?.Trim());
                                this.Output("Cумма кредита от общего дохода: " + reader.GetValue(4).ToString()?.Trim());
                                this.Output("Цель кредита: " + reader.GetValue(5).ToString()?.Trim());
                                this.Output("Срок кредита: " + reader.GetValue(6).ToString()?.Trim());
                                this.Output("Результат: " + reader.GetValue(7).ToString()?.Trim());
                            }
                            reader.Close();
                            sqlConn.Close();
                        }

                        break;
                    }
                    case "2":
                    {
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select _credit_summ_from_general_revenue, _credit_aim, _credit_term from [Faridun].[dbo].[users_application] where _is_payed = 0", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                this.Output("Cумма кредита от общего дохода: " + reader.GetValue(0).ToString()?.Trim());
                                this.Output("Цель кредита: " + reader.GetValue(1).ToString()?.Trim());
                                this.Output("Срок кредита: " + reader.GetValue(2).ToString()?.Trim());
                            }
                            reader.Close();
                            sqlConn.Close();
                        }

                        break;
                    }
                    case "3":
                    {
                        SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select _user_gender, _user_age, _married, _nationality, _credit_summ_from_general_revenue, _credit_aim, _credit_term from [Faridun].[dbo].[users_application] where _login={_name} and _status='OK'", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                this.Output("Пол: " + reader.GetValue(0).ToString()?.Trim());
                                this.Output("Возраст: " + reader.GetValue(1).ToString()?.Trim());
                                this.Output("Семейное положение: " + reader.GetValue(2).ToString()?.Trim());
                                this.Output("Гражданство: " + reader.GetValue(3).ToString()?.Trim());
                                this.Output("Cумма кредита от общего дохода: " + reader.GetValue(4).ToString()?.Trim());
                                this.Output("Цель кредита: " + reader.GetValue(5).ToString()?.Trim());
                                this.Output("Срок кредита: " + reader.GetValue(6).ToString()?.Trim());
                            }
                            reader.Close();
                            sqlConn.Close();
                        }

                        sqlConn = new SqlConnection(sqlManger.ConnectionString());
                        sqlConn.Open();
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            SqlCommand sqlCmd = new SqlCommand($"select _date, _summ from [Faridun].[dbo].[payment_list] where _login={_name}", sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                this.Output("Дата: " + reader.GetValue(0).ToString()?.Trim());
                                this.Output("Сумма: " + reader.GetValue(1).ToString()?.Trim());
                            }
                            reader.Close();
                            sqlConn.Close();
                        }

                        break;
                    }
                    case "4":
                        ApplicationInput(_name);
                        break;
                }
            }
        }

        ///<summary>
        ///Регистрация пользователя
        ///</summary>
        public void RegistrateOutput()
        {
            string login = string.Empty;
            while (login != "/Назад/")
            {
                login = Input("Введите свой номер телефона: ");
                string password = Input("Придумайте пароль: ");
                string passwordAgain = Input("Повторите пароль: ");
                if (password != passwordAgain) continue;
                string name = Input("Введите своё имя: ");
                string dateOfBirth = Input("Введите дату рождения, формат(dd-mm-yyyy): ");
                string homePath = Input("Введите прописку: ");
                string seria = Input("Введите серию паспорта: ");
                Authentication authentication = new Authentication(name, password);
                authentication.DateOfBirth = dateOfBirth;
                authentication.HomePath = homePath;
                authentication.LoginUser = int.Parse(login);
                bool reged = authentication.RegistrateAccount("users_list_table");
                if (!reged) continue;
                this.Output("Вы успешно зарегистрировались!");
                string agree = Input("Хотите подать заявку? (Д/Н): ");
                if (agree == "Д")
                    ApplicationInput(login);
                else
                    return;
            }
        }

        ///<summary>
        ///Регистрация заявки пользователя на кредит
        ///</summary>
        public void ApplicationInput(string login)
        {
            string userGender = Input("Пол(муж/жен): ");
            int userAge = Convert.ToInt32(Input("Возраст: "));
            string married = Input("Семейное положение(холост/семеянин/вразводе/вдовец/вдова): ");
            string nationality = Input("Гражданство (Таджикистан/Зарубеж): ");
            int creditSummFromGeneralRevenue = Convert.ToInt32(Input("Cумма кредита от общего дохода: "));
            string creditAim = Input("Цель кредита(бытовая техника/ремонт/телефон/прочее): ");
            int creditTerm = Convert.ToInt32(Input("Срок кредита: "));
            int creditSumm = Convert.ToInt32(Input("Сумма кредита: "));
            ClientApplication client = new ClientApplication(login);
            client.CreateApplication(userGender, married, userAge, nationality, creditSummFromGeneralRevenue, creditAim, creditTerm);
            bool isAccepted = client.AcceptedToCredit();
            string status = "NONE";

            status = isAccepted ? "OK" : "DISOK";

            SQLManager sqlManger = new SQLManager();
            sqlManger.UpdateData("users_application", $"_status='{status}'", $"_login={login}");
            string date = $"{DateTime.Now.Day.ToString()}.{DateTime.Now.Month.ToString()}.{DateTime.Now.Year.ToString()}";
            sqlManger.InsertData("payment_list", "_login, _date, _summ", $"'{login}', '{date}', {creditSumm}");
        }

        ///<summary>
        ///Внесение денег
        ///</summary>
        public void PaymentStory(float _summ, string _login)
        {
            SQLManager sqlManger = new SQLManager();
            string date = $"{DateTime.Now.Day.ToString()}.{DateTime.Now.Month.ToString()}.{DateTime.Now.Year.ToString()}";
            float toPlay = 0.0f;
            SqlConnection sqlConn = new SqlConnection(sqlManger.ConnectionString());
            sqlConn.Open();
            if (sqlConn.State == ConnectionState.Open)
            {
                SqlCommand sqlCmd = new SqlCommand($"select _summ from [Faridun].[dbo].[payment_list] where _login={_login}", sqlConn);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    toPlay = float.Parse(reader.GetValue(0).ToString()?.Trim().Replace('.', ',') ?? string.Empty);
                }
                reader.Close();
                sqlConn.Close();
            }

            _summ = toPlay - _summ;
            sqlManger.InsertData("payment_list", "_login, _date, _summ", $"{_login}, '{date}', {_summ}");
            if (_summ <= 0)
            {
                sqlManger.UpdateData("users_application", "_is_payed=1", $"_login={_login}");
            }
        }
    }
}