using System.Data.Entity;
using LibrarySystem.Domain.Models.DbModels;

namespace LibrarySystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<LoanRequest> LoanRequests { get; set; }
        public DbSet<LoanTransaction> LoanTransactions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public AppDbContext() : base("Data Source=P140\\SQLEXPRESS;Database=LibrarySystemDB;Integrated Security=True;") { }//Data Source=.;Initial Catalog=LibrarySystemDB;Integrated Security=True;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.SubscriptionTime)
                .IsRequired();

            // Configure Book
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            // Book - LoanTransaction: One-to-Many
            modelBuilder.Entity<LoanTransaction>()
                .HasRequired(lt => lt.Book)
                .WithMany(b => b.LoanTransactions)
                .HasForeignKey(lt => lt.BookId)
                .WillCascadeOnDelete(false); // Optional: prevent cascade delete if needed

            // one to many: User -> LoanRequests
            modelBuilder.Entity<LoanRequest>()
                .HasRequired(lr => lr.User)
                .WithMany(u => u.LoanRequests)
                .HasForeignKey(lr => lr.UserId);

            // 1-to-1: LoanRequest → LoanTransaction, using LoanTransaction.LoanRequestId as the FK
            modelBuilder.Entity<LoanRequest>()
                .HasOptional(r => r.LoanTransaction)
                .WithRequired(t => t.LoanRequest);

            // One LoanTransaction has many ActivityLogs
            modelBuilder.Entity<ActivityLog>()
                .HasRequired(a => a.LoanTransaction) // ✅ Correct: optional FK
                .WithMany(t => t.ActivityLogs)
                .HasForeignKey(a => a.LoanTransactionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fine>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Fine>()
                .HasRequired(f => f.LoanTransaction)
                .WithOptional(t => t.Fine)
                .WillCascadeOnDelete(false); 

        }
    }

}
