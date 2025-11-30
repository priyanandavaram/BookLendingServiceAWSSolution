using BookLendingServiceAWSSolution.Models;

namespace BookLendingServiceAWSSolution.Interface
{
    public interface IBookOperations
    {      
        void AddBook(Book book);
        public (string, bool) CheckoutBook(int bookId, string checkedoutUser);
        IEnumerable<Book> GetAllBooks();
        void ReturnBook(int bookId);
    }
}