using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class Friend
  {
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
  }
}
