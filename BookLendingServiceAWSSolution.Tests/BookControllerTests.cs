using BookLendingServiceAWSSolution.Controllers;
using BookLendingServiceAWSSolution.Interface;
using BookLendingServiceAWSSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookLendingServiceAWSSolution.Tests;

public class BookControllerTests
{

    private readonly Mock<IBookService> _bookServiceMock;
    private readonly BookController _controller;

    public BookControllerTests()
    {
        _bookServiceMock = new Mock<IBookService>();
        _controller = new BookController(_bookServiceMock.Object);
    }

    [Fact]
    public void AddNewBook_InvalidModel_ReturnsConflict()
    {
        var book = new Book();

        _controller.ModelState.AddModelError("BookTitle", "Required");

        var result = _controller.AddNewBook(book);

        var conflict = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal(409, conflict.StatusCode);
    }

    [Fact]
    public void AddNewBook_BookAlreadyExists_ReturnsConflict()
    {
        string bookTitle = "Mummy Returns";

        var book = new Book { BookTitle = bookTitle };

        _bookServiceMock.Setup(s => s.GetBookByName(bookTitle.ToLower()))
                        .Returns(true);

        var result = _controller.AddNewBook(book);

        var conflict = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal(409, conflict.StatusCode);
    }

    [Fact]
    public void AddNewBook_ValidBook_AddsBookAndReturnsOk()
    {
        string bookTitle = "Sherlock Holmes";

        var book = new Book { BookTitle = bookTitle };

        _bookServiceMock.Setup(s => s.GetBookByName(bookTitle))
                        .Returns(false);

        var result = _controller.AddNewBook(book);

        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);
    }

    [Fact]
    public void CheckoutBook_MissingUser_ReturnsBadRequest()
    {
        var result = _controller.CheckoutBook(1, "");

        var bad = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, bad.StatusCode);
    }

    [Fact]
    public void CheckoutBook_BookNotFound_ReturnsNotFound()
    {
        _bookServiceMock.Setup(s => s.GetBookById(1))
                    .Returns((Book)null);

        var result = _controller.CheckoutBook(1, "Sherley");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void CheckoutBook_AlreadyCheckedOut_ReturnsConflict()
    {
        var book = new Book 
        { 
            Id = 1, 
            BookTitle = "HarryPotter", 
            IsBookAvailable = false 
        };

        _bookServiceMock.Setup(s => s.GetBookById(1))
                        .Returns(book);

        var result = _controller.CheckoutBook(1, "Sherley");

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public void CheckoutBook_Valid_ReturnsOk()
    {
        var book = new Book
        {
            Id = 1,
            BookTitle = "HarryPotter",
            IsBookAvailable = true
        };

        _bookServiceMock.Setup(s => s.GetBookById(1))
                    .Returns(book);

        var result = _controller.CheckoutBook(1, "Sherley");

        var ok = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetAllBooks_ReturnsList()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, BookTitle = "Jones Part 1" },
            new Book { Id = 2, BookTitle = "Jones Part 2" }
        };

        _bookServiceMock.Setup(s => s.GetAllBooks())
                        .Returns(books);

        var result = _controller.GetAllBooks();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void ReturnBook_NotFound_ReturnsNotFound()
    {
        _bookServiceMock.Setup(s => s.GetBookById(1))
                        .Returns((Book)null);

        var result = _controller.ReturnBook(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnBook_AlreadyAvailable_ReturnsConflict()
    {
        var book = new Book 
        { 
            Id = 1, 
            BookTitle = "Jelly fish Story", 
            IsBookAvailable = true 
        };

        _bookServiceMock.Setup(s => s.GetBookById(1))
                    .Returns(book);

        var result = _controller.ReturnBook(1);

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public void ReturnBook_Valid_ReturnsOk()
    {
        var book = new Book 
        { 
            Id = 1, 
            BookTitle = "Test", 
            IsBookAvailable = false 
        };

        _bookServiceMock.Setup(s => s.GetBookById(1))
                    .Returns(book);

        var result = _controller.ReturnBook(1);

        var ok = Assert.IsType<OkObjectResult>(result);

        _bookServiceMock.Verify(s => s.ReturnBook(1), Times.Once);
    }
}