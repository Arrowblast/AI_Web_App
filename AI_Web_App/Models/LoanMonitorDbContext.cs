using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace AI_Web_App.Models
{
    public class LoanMonitorDbContext:DbContext
    {
        public DbSet<LoansMonitor> Monitors { get; set; }

        public LoanMonitorDbContext() : base("DefaultConnection") { }
    }
}