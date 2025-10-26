using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly IMemberService _memberService;
    private readonly IBookService _bookService;

    public TransactionController(ITransactionService transactionService, IMemberService memberService, IBookService bookService)
    {
        _transactionService = transactionService;
        _memberService = memberService;
        _bookService = bookService;
    }

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

    [Authorize]
    public IActionResult Borrow(int bookId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var member = _memberService.GetByUserId(userId);
        if (member == null) return RedirectToAction("Create", "Member");

        var book = _bookService.GetBookById(bookId);
        if (book == null || !book.IsAvailable) return NotFound("Book not available.");

        _transactionService.RecordBorrow(member.Id, bookId);
        book.IsAvailable = false;
        _bookService.UpdateBook(book);

        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Return(int transactionId)
    {
        var transaction = _transactionService.GetById(transactionId);
        if (transaction == null || transaction.DateReturned != null) return NotFound("Invalid transaction.");

        transaction.DateReturned = DateTime.Now;
        _transactionService.Update(transaction);

        var book = _bookService.GetBookById(transaction.BookId);
        if (book != null)
        {
            book.IsAvailable = true;
            _bookService.UpdateBook(book);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
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