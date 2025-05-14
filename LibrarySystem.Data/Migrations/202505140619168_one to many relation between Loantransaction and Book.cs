namespace LibrarySystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onetomanyrelationbetweenLoantransactionandBook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books");
            AddForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books");
            AddForeignKey("dbo.LoanTransactions", "BookId", "dbo.Books", "Id", cascadeDelete: true);
        }
    }
}
