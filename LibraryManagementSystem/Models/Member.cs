using System.ComponentModel.DataAnnotations;

public class Member
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    [Required]
    public string UserId { get; set; } // FK to IdentityUser

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } // ✅ Add this line

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}