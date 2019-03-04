using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class InviteDbContext:DbContext
    {
        public DbSet<Invite> Invites { get; set; }

        public InviteDbContext() : base("DefaultConnection") { }
    }
}