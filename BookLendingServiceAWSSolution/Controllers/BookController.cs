using BookLendingServiceAWSSolution.Interface;
using BookLendingServiceAWSSolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingServiceAWSSolution.Controllers;

[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }  

    [HttpPost]
    public IActionResult AddNewBook([FromBody] Book bookInfo)
    {
        if(!ModelState.IsValid)
        {
            return Conflict(new { message = "Invalid book information." });
        }
        else
        {
            var ifBookExists = _bookService.GetBookByName(bookInfo.BookTitle.ToLower());

            if (ifBookExists)
            {
                return Conflict(new { message = "A book with the same title already exists." });
            }
            else
            {
                _bookService.AddBook(bookInfo);

                return Ok(new { message = "Book has been added successfully.", book = bookInfo });

            }
        }
    }

    [HttpPost]
    [Route("{id:int:min(1)}/checkout")]
    public IActionResult CheckoutBook(int id,[FromBody] string checkedoutUser)
    {
        if (string.IsNullOrWhiteSpace(checkedoutUser))
        {
            return BadRequest(new { message = "Checked-out user must be provided." });
        }

        Book bookInfo = _bookService.GetBookById(id);

        if (bookInfo == null)
        {
            return NotFound(new { message = $"Book with id {id} not found." });
        }

        if (!bookInfo.IsBookAvailable)
        {
            return Conflict(new { message = "Book is already checked out." });
        }
        else
        {
            _bookService.CheckoutBook(id, checkedoutUser);

            return Ok(new { message = $"Book '{bookInfo.BookTitle}' checked out by {checkedoutUser}." });
        }
    }

    [HttpGet]
    public IEnumerable<Book> GetAllBooks()
    {
        return _bookService.GetAllBooks();
    }

    [HttpPost]
    [Route("{id:int:min(1)}/return")]
    public IActionResult ReturnBook(int id)
    {
        var bookInfo = _bookService.GetBookById(id);

        if (bookInfo == null)
        {
            return NotFound(new { message = $"Book with id {id} was not found." });
        }

        if (bookInfo.IsBookAvailable)
        {
            return Conflict(new { message = "Book is not currently checked out." });
        }
        else
        {
            _bookService.ReturnBook(id);

            return Ok(new { message = $"Book '{bookInfo.BookTitle}' has been returned successfully." });
        }
    }
}