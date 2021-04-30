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
            var userOne = new User {
                FirstName = "Andre",
                LastName = "Runner",
                EmailAddress = "andreeg@hiof.no",
                Password = "InsaneGamer",
                DateOfBirth = DateTime.Now
            };

            var userTwo = new User
            {
                FirstName = "Test",
                LastName = "Testy",
                EmailAddress = "test@test.com",
                Password = "test",
                DateOfBirth = DateTime.Now
            };

            var workspaceOne = new Workspace
            {
                Name = "My Workspace",
                DateAdded = DateTime.Now,
                CreatedByUser = userTwo
            };

            var projectOne = new Project
            {
                ProjectName = ".NET",
                Workspace = workspaceOne
            };

            //var tag_one = new Tag
            //{
            //    Name = "Random",
            //    Description = "Something"
            //};

           
            //session_one.Tags.Add(new SessionTag() { Session = session_one, Tag = tag_one });
            using var db = new ProductivityContext();

            db.Users.Add(userTwo);
            db.Workspaces.Add(workspaceOne);
            db.Projects.Add(projectOne);
            db.SaveChanges();

            var sessionOne = new Session
            {
                Description = "Writing some examples",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                UserId = userTwo.UserId,
                ProjectId = projectOne.ProjectId
            };

            db.Sessions.Add(sessionOne);

            db.SaveChanges();
        }
    }
}
