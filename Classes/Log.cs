using System;

namespace Bank_Credit_Manager
{
    class Log
    {
        public static void Error(string exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Fatal error was occured: " + exception);
        }
    }   
}