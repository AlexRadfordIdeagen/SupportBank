using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog.Config;
using NLog.Targets;
using NLog;

namespace SupportBank
{
    class Program
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

         static List<Transaction> transactions = new List<Transaction>();

        static void Main(string[] args)
        {
            InitializeLogging();

            CSVReader();

            InputCommand();

            Console.ReadLine();
        }

        private static void InputCommand()
        {
            Console.WriteLine("Enter your command");
            string command = Console.ReadLine();


            if (command.ToLower() == "list all")
            {
                var dictionary = AggregatePerEntry(transactions);
                PrintAggregation(dictionary);
            }
            else if (command.ToLower().Contains("list ["))
            {
                string[] subCommand = command.Split('[');
                string accountName = subCommand[1].Remove(subCommand[1].Length - 1);

                PrintAccountDetails(accountName, transactions);
            }
        }

        private static void CSVReader()
        {
            using (var reader = new StreamReader(@"C:\Users\Alex.Radford\source\Training\SupportBank\DodgyTransactions2015.csv"))
            {

                int i = 0;

                while (!reader.EndOfStream)
                {
                    var splits = reader.ReadLine().Split(',');
                    if (i != 0)
                    {

                        Transaction myTransaction = Transaction.TryToCreate(splits[0], splits[1], splits[2], splits[3], splits[4]);
                        if (myTransaction != null)
                        {
                            transactions.Add(myTransaction);
                        }
                    }
                    i++;
                }
            }
        }

        private static void InitializeLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = @"C:\Users\Alex.Radford\source\Training\SupportBank\SupportBank.log",
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
            log.Info("The application has started");
        }

        private static void PrintAccountDetails(string accountName, List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.To == accountName || transaction.From == accountName)
                {
                    transaction.Print();
                }
            }
        }

        private static void PrintAggregation(Dictionary<string, double> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine(item);
            }
        }

        private static Dictionary<string, double> AggregatePerEntry(List<Transaction> transactions)
        {
            var dictionary = new Dictionary<string, double>();

            foreach (var transaction in transactions)
            {
                if (dictionary.ContainsKey(transaction.To))
                {
                    dictionary[transaction.To] += transaction.Amount;
                }
                else
                {
                    dictionary.Add(transaction.To, transaction.Amount);
                }
                if (dictionary.ContainsKey(transaction.From))
                {
                    dictionary[transaction.From] -= transaction.Amount;
                }
                else
                {
                    dictionary.Add(transaction.From, -transaction.Amount);
                }

            }
            return dictionary;
        }
    }
}


