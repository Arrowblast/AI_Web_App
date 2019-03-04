namespace AI_Web_App.BooksCatalogMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class BooksCatalogConf : DbMigrationsConfiguration<AI_Web_App.Models.BooksCatalogDbContext>
    {
        public BooksCatalogConf()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"BooksCatalogMigrations";
        }

        protected override void Seed(AI_Web_App.Models.BooksCatalogDbContext context)
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
