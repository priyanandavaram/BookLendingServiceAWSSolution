namespace BookLendingServiceAWSSolution.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string BookTitle { get; set; }
        public required string BookAuthor { get; set; }
        public required string CheckedOutUser { get; set; }
        public required DateTime CheckedOutTime { get; set; }
        public bool IsBookAvailable { get; set; }        
    }
}