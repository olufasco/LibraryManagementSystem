public class BookService : IBookService
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public List<Book> GetAllBooks() => _books;
    public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);
    public void AddBook(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
    }
    public void RemoveBook(int id) => _books.RemoveAll(b => b.Id == id);
    public void UpdateBook(Book book)
    {
        var existing = GetBookById(book.Id);
        if (existing != null)
        {
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.IsAvailable = book.IsAvailable;
        }
    }
}