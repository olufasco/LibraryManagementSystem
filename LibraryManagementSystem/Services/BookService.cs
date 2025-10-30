public class BookService : IBookService
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public List<Book> GetAllBooks() => _books;

    public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public void AddBook(Book book)
    {
        book.Id = _nextId++;

        // Ensure AvailableCopies is initialized properly
        if (book.AvailableCopies <= 5)
        {
            book.AvailableCopies = book.TotalCopies;
        }

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

            // Optional: Adjust AvailableCopies only if TotalCopies changes
            if (book.TotalCopies != existing.TotalCopies)
            {
                int difference = book.TotalCopies - existing.TotalCopies;
                existing.AvailableCopies += difference;

                // Prevent negative available copies
                if (existing.AvailableCopies < 0)
                    existing.AvailableCopies = 0;
            }

            existing.TotalCopies = book.TotalCopies;
        }
    }
}