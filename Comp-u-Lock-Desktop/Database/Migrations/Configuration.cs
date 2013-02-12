using Database.Models;

namespace Database.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            AddComputers(context);
            AddUser(context);
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
        }

        private void AddComputers(DatabaseContext context)
        {

        }

        private void AddUser(DatabaseContext context)
        {
            context.Users.Add(new User
                {
                    AuthToken = "assdfsfgwfdf",
                    Computers = null,
                    DateCreated = DateTime.Now,
                    Email = "crouska@gmail.com",
                    Id = 0,
                    LastUpdated = DateTime.Now,
                    Username = "Rizowski"
                });
            ;
        }
    }
}
