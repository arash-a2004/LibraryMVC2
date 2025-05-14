namespace LibrarySystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fines",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LoanTransactionId = c.Int(nullable: false),
                        DaysLate = c.Int(nullable: false),
                        FineAmount = c.Int(nullable: false),
                        FineDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanTransactions", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fines", "Id", "dbo.LoanTransactions");
            DropIndex("dbo.Fines", new[] { "Id" });
            DropTable("dbo.Fines");
        }
    }
}
