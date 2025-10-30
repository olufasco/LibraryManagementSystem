using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using System;

[Authorize]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly IMemberService _memberService;
    private readonly IBookService _bookService;

    public TransactionController(
        ITransactionService transactionService,
        IMemberService memberService,
        IBookService bookService)
    {
        _transactionService = transactionService;
        _memberService = memberService;
        _bookService = bookService;
    }

    // Admin: View all transactions
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var transactions = _transactionService.GetAllTransactions();
        foreach (var t in transactions)
        {
            t.Member = _memberService.GetMemberById(t.MemberId);
            t.Book = _bookService.GetBookById(t.BookId);
        }
        return View(transactions);
    }

    // Member: Borrow a book
    [Authorize(Roles = "Member")]
    public IActionResult Borrow(int bookId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var member = _memberService.GetByUserId(userId);
        if (member == null) return RedirectToAction("Create", "Member");

        var book = _bookService.GetBookById(bookId);
        if (book == null || book.AvailableCopies < 1)
        {
            TempData["Debug"] = $"AvailableCopies: {book?.AvailableCopies}, TotalCopies: {book?.TotalCopies}";

            return RedirectToAction("Index", "Book");
        }

        _transactionService.RecordBorrow(member.Id, bookId);
        book.AvailableCopies--; // ✅ Decrease available copies
        _bookService.UpdateBook(book);

        TempData["Success"] = "Book borrowed successfully.";
        return RedirectToAction("MyBorrowedBooks");
    }

    // Member: Return a book
    [Authorize(Roles = "Member")]
    public IActionResult Return(int transactionId)
    {
        var transaction = _transactionService.GetById(transactionId);
        if (transaction == null || transaction.DateReturned != null) return NotFound("Invalid transaction.");

        transaction.DateReturned = DateTime.Now;
        _transactionService.Update(transaction);

        var book = _bookService.GetBookById(transaction.BookId);
        if (book != null)
        {
            book.AvailableCopies++; // ✅ Increase available copies
            _bookService.UpdateBook(book);
        }

        TempData["Success"] = "Book returned successfully.";
        return RedirectToAction("MyBorrowedBooks");
    }

    // Member: View borrowed books
    [Authorize(Roles = "Member")]
    public IActionResult MyBorrowedBooks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var member = _memberService.GetByUserId(userId);
        if (member == null) return RedirectToAction("Create", "Member");

        var transactions = _transactionService.GetByMemberId(member.Id);
        foreach (var t in transactions)
        {
            t.Book = _bookService.GetBookById(t.BookId);
        }

        return View(transactions);
    }
}