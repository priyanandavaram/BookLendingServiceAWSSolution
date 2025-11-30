using BookLendingServiceAWSSolution.Interface;
using BookLendingServiceAWSSolution.Models;

namespace BookLendingServiceAWSSolution.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>();

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public (string, bool) CheckoutBook(int bookId, string checkedoutUser)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                if (book.IsBookAvailable)
                {
                    book.IsBookAvailable = false;
                    book.CheckedOutUser = checkedoutUser;
                    book.CheckedOutTime = DateTime.Now;
                    return ("Book checkout successfully", true);
                }
                else
                {
                    return ("Book is unavaible", false);
                }
            }
            else
            {
                return ("Book doesn't exists with that name", false);
            }

        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
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