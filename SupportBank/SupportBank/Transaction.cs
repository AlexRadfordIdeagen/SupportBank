using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class Transaction
    {
        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Narrative { get; set; }
        public double Amount { get; set; }
         


        public Transaction(string Date, string From, string To, string Narrative, string Amount)
        {
            this.Date = Date;
            this.From = From;
            this.To = To;
            this.Narrative = Narrative;
            this.Amount = Convert.ToDouble(Amount);
            
        }
    }
    

}
