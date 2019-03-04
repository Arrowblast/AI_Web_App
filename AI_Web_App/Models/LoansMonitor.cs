using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class LoansMonitor
    {
        private string name;
        private DateTime loanDate;
        private DateTime returnDate;
        private int id;
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime LoanDate { get => loanDate; set => loanDate = value; }
        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }
        
    }
}