using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IBorrowService _borrowService;
    private readonly UserManager<IdentityUser> _userManager;

    public BookController(
        IBookService bookService,
        IBorrowService borrowService,
        UserManager<IdentityUser> userManager)
    {
        _bookService = bookService;
        _borrowService = borrowService;
        _userManager = userManager;
    }

    // Shared: Members and Admins can view books
    [Authorize(Roles = "Member,Admin")]
    public IActionResult Index()
    {
        var books = _bookService.GetAllBooks();
        return View(books);
    }

    // Member: Submit borrow request
    [Authorize(Roles = "Member")]
    public IActionResult RequestBorrow(int bookId)
    {
        var request = new BorrowRequest
        {
            MemberId = _userManager.GetUserId(User),
            BookId = bookId,
            RequestDate = DateTime.Now,
            IsApproved = false
        };
        _borrowService.SubmitRequest(request);
        return RedirectToAction("MyBorrowedBooks", "Transaction");
    }

    // Admin only: Add book
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.AddBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    // Admin only: Edit book
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var book = _bookService.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.UpdateBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    // Admin only: Delete book
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var book = _bookService.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(Book book)
    {
        _bookService.RemoveBook(book.Id);
        return RedirectToAction("Index");
    }
}