using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SupportBank
{
    class Transaction
    {
        public string Date { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Narrative { get; set; }
        public double Amount { get; set; }
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        public static Transaction TryToCreate(string Date, string From, string To, string Narrative, string Amount)
        {


            try
            {
                Convert.ToDouble(Amount);
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
            return new Transaction(Date, From, To, Narrative, Convert.ToDouble(Amount));

        }


        public Transaction(string Date, string From, string To, string Narrative, double Amount)
        {
            this.Date = Date;
            this.FromAccount = From;
            this.ToAccount = To;
            this.Narrative = Narrative;
            this.Amount = Amount;

        }

        public void Print()
        {
            Console.WriteLine(Date + " " + FromAccount + " " + ToAccount + " " + Narrative + " " + Amount);
        }
    }


}
