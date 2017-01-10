namespace urednistvo.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<urednistvo.Models.UrednistvoDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "urednistvo.Models.UrednistvoDatabase";
        }

        protected override void Seed(urednistvo.Models.UrednistvoDatabase context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var editions = new List<Edition> {
                new Edition { TimeOfRelease = DateTime.Now },
                new Edition { TimeOfRelease = DateTime.Now.AddMonths(1) }
            };
            editions.ForEach(s => context.Editions.AddOrUpdate(p => p.TimeOfRelease, s));
            context.SaveChanges();
        }
    }
}
