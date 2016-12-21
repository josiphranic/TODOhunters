using System.Collections.Generic;

namespace urednistvo.ModelsDB
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } //enum

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Text> Texts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}