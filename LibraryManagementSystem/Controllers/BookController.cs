using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using System;

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

    [Authorize(Roles = "Member,Admin")]
    public IActionResult Index()
    {
        var books = _bookService.GetAllBooks();
        return View(books);
    }

    [Authorize(Roles = "Member")]
    public IActionResult RequestBorrow(int bookId)
    {
        var book = _bookService.GetBookById(bookId);
        if (book == null || book.AvailableCopies < 1)
        {
            TempData["Error"] = "Book is not available.";
            return RedirectToAction("Index");
        }

        var request = new BorrowRequest
        {
            MemberId = _userManager.GetUserId(User),
            BookId = bookId,
            RequestDate = DateTime.Now,
            IsApproved = false
        };

        _borrowService.SubmitRequest(request);
        TempData["Success"] = "Borrow request submitted.";
        return RedirectToAction("MyBorrowedBooks", "Transaction");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.AddBook(book);
            TempData["Success"] = "Book added.";
            return RedirectToAction("Index");
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null) return NotFound();
        return View(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.UpdateBook(book);
            TempData["Success"] = "Book updated.";
            return RedirectToAction("Index");
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null) return NotFound();
        return View(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(Book book)
    {
        _bookService.RemoveBook(book.Id);
        TempData["Success"] = "Book deleted.";
        return RedirectToAction("Index");
    }
}