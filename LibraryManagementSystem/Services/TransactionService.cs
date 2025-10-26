public class TransactionService : ITransactionService
{
    private readonly List<BorrowTransaction> _transactions = new();
    private int _nextId = 1;

    public List<BorrowTransaction> GetAllTransactions() => _transactions;
    public BorrowTransaction GetById(int id) => _transactions.FirstOrDefault(t => t.Id == id);
    public List<BorrowTransaction> GetByMemberId(int memberId) =>
        _transactions.Where(t => t.MemberId == memberId).ToList();

    public void RecordBorrow(int memberId, int bookId)
    {
        _transactions.Add(new BorrowTransaction
        {
            Id = _nextId++,
            MemberId = memberId,
            BookId = bookId,
            DateBorrowed = DateTime.Now
        });
    }

    public void Update(BorrowTransaction transaction)
    {
        var existing = GetById(transaction.Id);
        if (existing != null)
        {
            existing.DateReturned = transaction.DateReturned;
        }
    }
}