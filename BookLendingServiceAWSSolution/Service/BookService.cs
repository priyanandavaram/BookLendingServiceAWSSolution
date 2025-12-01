using BookLendingServiceAWSSolution.Interface;
using BookLendingServiceAWSSolution.Models;

namespace BookLendingServiceAWSSolution.Service
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public void AddBook(Book book)
        {
            IEnumerable<Book> getBooks = _bookRepository.GetAllBooks();

            int getLastBookId = getBooks.OrderByDescending(bookInfo => bookInfo.Id)
                                         .Select(bookInfo => bookInfo.Id)
                                         .FirstOrDefault();

            book.Id = getLastBookId + 1;

            _bookRepository.AddBook(book);
        }

        public void CheckoutBook(int bookId, string checkedoutUser)
        {
             _bookRepository.CheckoutBook(bookId, checkedoutUser);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }

        public bool GetBookByName(string bookName)
        {
            return _bookRepository.GetBookByName(bookName);
        }

        public void ReturnBook(int bookId)
        {
            _bookRepository.ReturnBook(bookId);
        }

        Book IBookOperations.AddBook(Book book)
        {
            throw new NotImplementedException();
        }
    }
}