using BookLendingServiceAWSSolution.Interface;
using BookLendingServiceAWSSolution.Models;

namespace BookLendingServiceAWSSolution.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>();

        public Book AddBook(Book book)
        {
            _books.Add(book);

            return GetBookById(book.Id);
        }

        public void CheckoutBook(int bookId, string checkedoutUser)
        {
            var bookInfo = GetBookById(bookId);

            bookInfo.IsBookAvailable = false;
            bookInfo.CheckedOutUser = checkedoutUser;
            bookInfo.CheckedOutTime = DateTime.Now;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
        }

        public Book GetBookById(int bookId)
        {
            return _books.FirstOrDefault(bookInfo => bookInfo.Id == bookId);
        }

        public bool GetBookByName(string bookName)
        {
            bool checkIfBookExists = _books.Any(bookInfo => bookInfo.BookTitle.Equals(bookName, StringComparison.OrdinalIgnoreCase));

            return checkIfBookExists;
        }

        public void ReturnBook(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                if (!book.IsBookAvailable)
                {
                    book.IsBookAvailable = true;
                }
            }

        }
    }
}