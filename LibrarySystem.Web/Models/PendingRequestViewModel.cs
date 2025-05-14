using System;

namespace LibrarySystem.Web.Models
{
    public class PendingRequestViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string BookTitle { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }

}