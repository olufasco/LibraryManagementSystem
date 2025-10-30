using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BorrowTransaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    public DateTime DateBorrowed { get; set; } = DateTime.Now;

    public DateTime? DateReturned { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; }

    public bool IsOverdue => DateReturned == null && DateTime.Now > DueDate;

    [ForeignKey("BookId")]
    public Book Book { get; set; }

    [ForeignKey("MemberId")]
    public Member Member { get; set; }
}