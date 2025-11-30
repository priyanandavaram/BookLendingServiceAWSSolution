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
    public void AddNewBook(Book bookInfo)
    {
        _bookService.AddBook(bookInfo);
    }

    [HttpPost]
    [Route("{id:int:min(1)}")]
    public void CheckoutBook(int id,[FromBody] string checkedoutUser)
    {
        _bookService.CheckoutBook(id, checkedoutUser);
    }

    [HttpGet]
    public IEnumerable<Book> GetAllBooks()
    {
        return _bookService.GetAllBooks();
    }

    [HttpPost]
    [Route("{id:int:min(1)}")]
    public void ReturnBook(int id)
    {
        _bookService.ReturnBook(id);
    }
}