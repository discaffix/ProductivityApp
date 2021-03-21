using ProductivityApp.DataAccess;
using ProductivityApp.Model;
using System;

namespace ProductivityApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // user object
            var user = new User
            {
                FirstName = "Andre",
                LastName = "Runner",
                EmailAddress = "andreeg@hiof.no",
                Password = "InsaneGamer",
                DateOfBirth = DateTime.Now
            };

            // session object
            var session = new Session
            {
                Description = "Adding a DBContext",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };

            var workspace = new Workspace
            {
                Name = "My Workspace",
                DateAdded = DateTime.Now  
            };

            using var db = new ProductivityContext();

            user.Sessions.Add(session);




            db.Users.Add(user);

            db.SaveChanges();



        }
    }
}
