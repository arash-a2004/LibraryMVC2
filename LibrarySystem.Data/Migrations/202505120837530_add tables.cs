namespace LibrarySystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LoanTransactionId = c.Int(nullable: false),
                        Action = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.LoanTransactions", t => t.LoanTransactionId)
                .Index(t => t.UserId)
                .Index(t => t.LoanTransactionId);
            
            CreateTable(
                "dbo.LoanTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        LoanDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        LoanRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.LoanRequests", t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LoanRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            AddColumn("dbo.Books", "IsAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityLogs", "LoanTransactionId", "dbo.LoanTransactions");
            DropForeignKey("dbo.LoanTransactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.LoanTransactions", "Id", "dbo.LoanRequests");
            DropForeignKey("dbo.LoanRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.LoanRequests", "BookId", "dbo.Books");
            DropForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books");
            DropIndex("dbo.LoanRequests", new[] { "BookId" });
            DropIndex("dbo.LoanRequests", new[] { "UserId" });
            DropIndex("dbo.LoanTransactions", new[] { "UserId" });
            DropIndex("dbo.LoanTransactions", new[] { "BookId" });
            DropIndex("dbo.LoanTransactions", new[] { "Id" });
            DropIndex("dbo.ActivityLogs", new[] { "LoanTransactionId" });
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropColumn("dbo.Users", "IsActive");
            DropColumn("dbo.Books", "IsAvailable");
            DropTable("dbo.LoanRequests");
            DropTable("dbo.LoanTransactions");
            DropTable("dbo.ActivityLogs");
        }
    }
}
