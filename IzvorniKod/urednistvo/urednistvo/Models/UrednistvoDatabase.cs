using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace urednistvo.Models
{
    public class UrednistvoDatabase : DbContext
    {
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Text> Texts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Edition> Editions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
               .HasMany<User>(n => n.Users)
               .WithMany(u => u.Notifications)
               .Map(cs =>
               {
                   cs.MapLeftKey("NotificationId");
                   cs.MapRightKey("UserId");
                   cs.ToTable("UserNotification");
               });
            
        }
    }
}