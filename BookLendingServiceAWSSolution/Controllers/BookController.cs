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
    public void AddNewBook([FromBody] Book bookInfo)
    {
        //check if the model is valid?
        //validate if there is any book with the same name exists?
        _bookService.AddBook(bookInfo);
    }

    [HttpPost]
    [Route("{id:int:min(1)}/checkout")]
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
    [Route("{id:int:min(1)}/return")]
    public void ReturnBook(int id)
    {
        _bookService.ReturnBook(id);
    }
}