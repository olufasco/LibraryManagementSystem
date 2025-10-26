using Microsoft.AspNetCore.Mvc;

public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index() => View(_bookService.GetAllBooks());

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.AddBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    public IActionResult Edit(int id) => View(_bookService.GetBookById(id));

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _bookService.UpdateBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    public IActionResult Delete(int id) => View(_bookService.GetBookById(id));

    [HttpPost]
    public IActionResult Delete(Book book)
    {
        _bookService.RemoveBook(book.Id);
        return RedirectToAction("Index");
    }
}