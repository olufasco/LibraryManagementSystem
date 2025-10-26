public interface IBookService
{
    List<Book> GetAllBooks();
    Book GetBookById(int id);
    void AddBook(Book book);
    void RemoveBook(int id);
    void UpdateBook(Book book);
}