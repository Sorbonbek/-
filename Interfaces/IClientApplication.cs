namespace Bank_Credit_Manager
{
    interface IClientApplication
    {
        void CreateApplication(string _gender, string _isMarried, int _age, string _nation, int _creditSumm, string _creditAim, int _creditTerm);
        int CreditsCount();
        int CreditArrearage();
        bool AcceptedToCredit();
    }
}