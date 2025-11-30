namespace BookLendingServiceAWSSolution.Models
{
    public class Book
    {
        public int Id { get; set; }
        public  string BookTitle { get; set; } = string.Empty;
        public  string BookAuthor { get; set; } = string.Empty;
        public string CheckedOutUser { get; set; } = string.Empty;
        public DateTime CheckedOutTime { get; set; } = DateTime.Now;
        public bool IsBookAvailable { get; set; } = true;
    }
}