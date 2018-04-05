using System;
using NLog;

namespace SupportBank
{
    class Transaction
    {
        public DateTime Date { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Narrative { get; set; }
        public double Amount { get; set; }
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static Transaction TryToCreateFromCSV(string Date, string From, string To, string Narrative, string Amount)
        {
            try
            {
                Convert.ToDouble(Amount);
                DateTime.Parse(Date);
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
            return new Transaction(DateTime.Parse(Date), From, To, Narrative, Convert.ToDouble(Amount));
        }

        public static Transaction TryToCreateFromXML(double OADateNumber, string From, string To, string Narrative, string Amount)
        {
            try
            {
                Convert.ToDouble(Amount);
                DateTime.FromOADate(OADateNumber);
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
            return new Transaction(DateTime.FromOADate(OADateNumber), From, To, Narrative, Convert.ToDouble(Amount));
        }

        public Transaction(DateTime Date, string From, string To, string Narrative, double Amount)
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
