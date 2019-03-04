using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace AI_Web_App.Models
{
    public class CommentDbContext:DbContext
    {
        public DbSet<Comments> Comments { get; set; }

        public CommentDbContext() : base("DefaultConnection") { }
    }
}