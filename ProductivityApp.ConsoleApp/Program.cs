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
            var userOne = new User
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "sample@test.com",
                Password = "Password",
                DateOfBirth = DateTimeOffset.Now
            };

            var userTwo = new User
            {
                FirstName = "Test",
                LastName = "Testy",
                EmailAddress = "test@test.com",
                Password = "test",
                DateOfBirth = DateTimeOffset.Now
            };

            var workspaceOne = new Workspace
            {
                Name = "My Workspace",
                DateAdded = DateTimeOffset.Now,
                CreatedByUser = userTwo
            };

            var projectOne = new Project
            {
                ProjectName = ".NET",
                Workspace = workspaceOne
            };

            var projectTwo = new Project()
            {
                ProjectName = "Facebook NEXT",
                Workspace = workspaceOne
            };

            var projectThree = new Project()
            {
                ProjectName = "Scaffolding Platform",
                Workspace = workspaceOne
            };
                
            using var db = new ProductivityContext();

            var tagOne = new Tag()
            {
                Name = "University"
            };
           
            var tagTwo = new Tag()
            {
                Name = "Private Projects"
            };

            var tagThree = new Tag()
            {
                Name = "Portfolio"
            };

            db.Users.Add(userOne);
            db.Workspaces.Add(workspaceOne);
            db.Projects.Add(projectTwo);
            db.Projects.Add(projectThree);
            db.Projects.Add(projectOne);

            db.Tags.Add(tagOne);
            db.Tags.Add(tagTwo);
            db.Tags.Add(tagThree);

            db.SaveChanges();

            var sessionOne = new Session
            {
                Description = "Redesigning MainView in ProductivityApp.AppTesting",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(5),
                UserId = userTwo.UserId,
                ProjectId = projectOne.ProjectId
            };

            db.Sessions.Add(sessionOne);

            db.SaveChanges();
        }
    }
}
