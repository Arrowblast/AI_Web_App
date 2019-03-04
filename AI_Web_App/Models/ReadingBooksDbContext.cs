using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace AI_Web_App.Models
{
    public class ReadingBooksDbContext:DbContext
    {
        public DbSet<ReadingBooks> ReadingBooks { get; set; }

        public ReadingBooksDbContext() : base("DefaultConnection") { }
    }
}