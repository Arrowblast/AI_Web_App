using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AI_Web_App.Models
{
    public class BooksCatalogDbContext:DbContext
    {
        public DbSet<BookCatalog> Catalogs { get; set; }
        
        public BooksCatalogDbContext() : base("DefaultConnection") { }
    }
}