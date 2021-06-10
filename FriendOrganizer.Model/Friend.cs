using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class Friend
  {
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    public int Id { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }
  }
}
