using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace urednistvo.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage ="OJ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "OJ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "OJ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "OJ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OJ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int Role { get; set; } //enum

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Text> Texts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}