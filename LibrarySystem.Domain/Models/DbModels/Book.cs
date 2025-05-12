using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public virtual ICollection<LoanTransaction> LoanTransactions { get; set; }

    }

}
