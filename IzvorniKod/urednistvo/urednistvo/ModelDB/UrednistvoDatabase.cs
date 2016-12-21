using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace urednistvo.ModelsDB
{
    public class UrednistvoDatabase : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Persons { get; set; }
        public DbSet<Edition> Editions { get; set; }
    }
}