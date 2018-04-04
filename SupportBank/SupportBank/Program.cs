using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {


            List<Transaction> transactions = new List<Transaction>();

            using (var reader = new StreamReader(@"C:\Users\Alex.Radford\source\Training\SupportBank\Transactions2014.csv"))
            {


                while (!reader.EndOfStream)
                {
                    var splits = reader.ReadLine().Split(',');
                    Transaction myTranscation = new Transaction(splits[0],splits[1],splits[2],splits[3],splits[4]);
                    transactions.Add(myTranscation);

                }
            }
            Console.WriteLine("Enter your command");
            string command = Console.ReadLine().ToLower();
            
            if (command == "list all")
            {
                var dic = AggregatePerEntry(transactions);
                PrintAggregation(dic);
            }

          
            Console.ReadLine();
        }

        private static void PrintAggregation(Dictionary<string, double> dic)
        {
            foreach (var item in dic)
            {
                Console.WriteLine(item);
            }
        }

        private static Dictionary<string, double> AggregatePerEntry(List<Transaction> transactions)
        {
            var dictionary = new Dictionary<string, double>();

            foreach (var transaction in transactions)
            {
                if(dictionary.ContainsKey(transaction.To))
                {
                    dictionary[transaction.To] += transaction.Amount;
                }
                if (dictionary.ContainsKey(transaction.From))
                {
                    dictionary[transaction.To] -= transaction.Amount;
                }



                else
                    dictionary.Add(transaction.To, transaction.Amount);
               
            }
            return dictionary;
        }
    }
}


