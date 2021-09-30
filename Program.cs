namespace Bank_Credit_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInterface UI = new UserInterface();
            UI.Output("Здравствуйте! Хотите получить кредит за 2 минуты? Подайте заявку!!!");
            UI.Output("Выберите что хотите сделать: ");
            UI.Output("1. Войти");
            UI.Output("2. Регистрация");
            UI.Output("3. Внести деньги");
            UI.Output("0. Закрыть программу");
            string cmd = UI.Input("Сделайте выбор (1,2,3,0): ");
            while(cmd != "0")
            {
                switch (cmd)
                {
                    case "1":
                        UI.LoginOutput();
                        break;
                    case "2":
                        UI.RegistrateOutput();
                        break;
                    case "3":
                    {
                        string _login = string.Empty;
                        while (_login != "/Назад/")
                        {
                            _login = UI.Input("Введите логин(номер телефона): ").Replace('.', ',');
                            if (_login == "/Назад/") continue;
                            float _summ = float.Parse(UI.Input("Введите сумму: ").Replace('.', ','));
                            UI.PaymentStory(_summ, _login);
                        }

                        break;
                    }
                }
            }
        }
    }
}
