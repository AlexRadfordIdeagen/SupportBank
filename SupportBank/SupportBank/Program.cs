using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog.Config;
using NLog.Targets;
using NLog;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;



namespace SupportBank
{
    class Program
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static List<Transaction> transactions = new List<Transaction>();

        static void Main(string[] args)
        {
            InitializeLogging();

            InputFile();

            Console.ReadLine();
        }


        private static void InputFile()
        {
            Console.WriteLine("Import your file, e.g. Blarg.json");
            string file = Console.ReadLine();

            string extension = Path.GetExtension(file);

            if (extension == ".json")
            {
                JSONReader(file);
            }
           else  if (extension == ".csv")
            {
                CSVReader(file);
            }
           else  if (extension == ".xml")
            {
                XMLReader(file);
            }
            else
            {
                Console.WriteLine("I'm sorry " + file + " is not valid");
                InputFile();
            }

            InputCommand();
        }

        private static void WriteToFile(string write)
        {
            Console.WriteLine("Would you like to write the transaction log?  Y/N");

        }

        private static void XMLReader(string fileName)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Alex.Radford\source\Training\SupportBank\" + fileName);
            string Narrative;
            string Amount;
            string To;
            string From;

            foreach (XmlNode node in doc.DocumentElement.SelectSingleNode("//TransactionList"))
            {
                // TODO exception handling if parse fails
                var OADateNumber = Double.Parse(node.Attributes[0].InnerText);

                foreach (XmlNode child in node.ChildNodes)
                {

                    Narrative = node.ChildNodes[0].InnerText;
                    Amount = node.ChildNodes[1].InnerText;
                    From = node.ChildNodes[2].ChildNodes[0].InnerText;
                    To = node.ChildNodes[2].ChildNodes[1].InnerText;
                    Transaction myTransaction = Transaction.TryToCreateFromXML(OADateNumber, From, To, Narrative, Amount);
                    transactions.Add(myTransaction);
                }


            }



        }
        private static void JSONReader(string fileName)
        {

            using (StreamReader reader = new StreamReader(@"C:\Users\Alex.Radford\source\Training\SupportBank\" + fileName))
            {
                transactions = JsonConvert.DeserializeObject<List<Transaction>>(reader.ReadToEnd());
            }

        }

        private static void CSVReader(string fileName)
        {
            using (var reader = new StreamReader(@"C:\Users\Alex.Radford\source\Training\SupportBank\" + fileName))
            {

                int i = 0;

                while (!reader.EndOfStream)
                {
                    var splits = reader.ReadLine().Split(',');
                    if (i != 0)
                    {

                        Transaction myTransaction = Transaction.TryToCreateFromCSV(splits[0], splits[1], splits[2], splits[3], splits[4]);
                        if (myTransaction != null)
                        {
                            transactions.Add(myTransaction);
                        }
                    }
                    i++;
                }
            }
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
                if (transaction.ToAccount == accountName || transaction.FromAccount == accountName)
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
                if (dictionary.ContainsKey(transaction.ToAccount))
                {
                    dictionary[transaction.ToAccount] += transaction.Amount;
                }
                else
                {
                    dictionary.Add(transaction.ToAccount, transaction.Amount);
                }
                if (dictionary.ContainsKey(transaction.FromAccount))
                {
                    dictionary[transaction.FromAccount] -= transaction.Amount;
                }
                else
                {
                    dictionary.Add(transaction.FromAccount, -transaction.Amount);
                }

            }
            return dictionary;
        }
    }
}


