namespace LibrarySystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLoanTransaction : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityLogs", new[] { "LoanTransactionId" });
            AlterColumn("dbo.ActivityLogs", "LoanTransactionId", c => c.Int());
            CreateIndex("dbo.ActivityLogs", "LoanTransactionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityLogs", new[] { "LoanTransactionId" });
            AlterColumn("dbo.ActivityLogs", "LoanTransactionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ActivityLogs", "LoanTransactionId");
        }
    }
}
