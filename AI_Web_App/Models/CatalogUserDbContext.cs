using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class CatalogUserDbContext:DbContext
    {
        public DbSet<CatalogUser> CatalogUsers { get; set; }

        public CatalogUserDbContext() : base("DefaultConnection") { }
    }
}