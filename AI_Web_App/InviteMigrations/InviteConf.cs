namespace AI_Web_App.InviteMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class InviteConf : DbMigrationsConfiguration<AI_Web_App.Models.InviteDbContext>
    {
        public InviteConf()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"InviteMigrations";
        }

        protected override void Seed(AI_Web_App.Models.InviteDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
