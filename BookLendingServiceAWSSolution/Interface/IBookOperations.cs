using BookLendingServiceAWSSolution.Models;

namespace BookLendingServiceAWSSolution.Interface
{
    public interface IBookOperations
    {      
        Book AddBook(Book book);
        public void CheckoutBook(int bookId, string checkedoutUser);
        IEnumerable<Book> GetAllBooks();
        bool GetBookByName(string bookName);
        Book GetBookById (int bookId);
        void ReturnBook(int bookId);
    }
}