namespace AI_Web_App.BooksCatalogMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookCatalogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Owner = c.String(),
                        Artist = c.String(),
                        Wydawnictwo = c.String(),
                        Link = c.String(),
                        TrueOwner = c.String(),
                        Loan = c.Int(nullable: false),
                        Reading = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookCatalogs");
        }
    }
}
