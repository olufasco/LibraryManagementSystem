
public interface ITransactionService
{
    //IEnumerable<object> GetAllBorrowedTransactions();
    List<BorrowTransaction> GetAllTransactions();
    BorrowTransaction GetById(int id);
    List<BorrowTransaction> GetByMemberId(int memberId);
    void RecordBorrow(int memberId, int bookId);
    void Update(BorrowTransaction transaction);
}