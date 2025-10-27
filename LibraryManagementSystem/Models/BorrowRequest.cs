using Microsoft.AspNetCore.Identity;

public class BorrowRequest
{
    public int Id { get; set; }
    public string MemberId { get; set; } // FK to IdentityUser
    public int BookId { get; set; }
    public DateTime RequestDate { get; set; }
    public bool IsApproved { get; set; } = false;

    public IdentityUser Member { get; set; }
    public Book Book { get; set; }
}