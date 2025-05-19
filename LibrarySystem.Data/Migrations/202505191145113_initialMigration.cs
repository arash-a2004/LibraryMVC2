namespace LibrarySystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LoanTransactionId = c.Int(),
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
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.LoanRequests", t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Author = c.String(nullable: false, maxLength: 100),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fines",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LoanTransactionId = c.Int(nullable: false),
                        DaysLate = c.Int(),
                        FineAmount = c.Decimal(precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanTransactions", t => t.Id)
                .Index(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false),
                        Role = c.Int(nullable: false),
                        SubscriptionTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityLogs", "LoanTransactionId", "dbo.LoanTransactions");
            DropForeignKey("dbo.LoanTransactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.LoanRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.LoanTransactions", "Id", "dbo.LoanRequests");
            DropForeignKey("dbo.LoanRequests", "BookId", "dbo.Books");
            DropForeignKey("dbo.Fines", "Id", "dbo.LoanTransactions");
            DropForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books");
            DropIndex("dbo.LoanRequests", new[] { "BookId" });
            DropIndex("dbo.LoanRequests", new[] { "UserId" });
            DropIndex("dbo.Fines", new[] { "Id" });
            DropIndex("dbo.LoanTransactions", new[] { "UserId" });
            DropIndex("dbo.LoanTransactions", new[] { "BookId" });
            DropIndex("dbo.LoanTransactions", new[] { "Id" });
            DropIndex("dbo.ActivityLogs", new[] { "LoanTransactionId" });
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.LoanRequests");
            DropTable("dbo.Fines");
            DropTable("dbo.Books");
            DropTable("dbo.LoanTransactions");
            DropTable("dbo.ActivityLogs");
        }
    }
}
