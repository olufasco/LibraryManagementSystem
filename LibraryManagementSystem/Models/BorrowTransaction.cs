public class BorrowTransaction
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime DateBorrowed { get; set; } = DateTime.Now;
    public DateTime? DateReturned { get; set; }

    public Member Member { get; set; }
    public Book Book { get; set; }
}