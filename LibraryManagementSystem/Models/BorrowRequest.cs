using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class BorrowRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string MemberId { get; set; } // FK to IdentityUser

    [Required]
    public int BookId { get; set; }

    public DateTime RequestDate { get; set; } = DateTime.Now;

    public DateTime? DueDate { get; set; } // Set when approved

    public bool IsApproved { get; set; } = false;

    public bool IsReturned { get; set; } = false;

    [ForeignKey("MemberId")]
    public IdentityUser Member { get; set; }

    [ForeignKey("BookId")]
    public Book Book { get; set; }
}