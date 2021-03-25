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
            var user_one = new User {
                FirstName = "Andre",
                LastName = "Runner",
                EmailAddress = "andreeg@hiof.no",
                Password = "InsaneGamer",
                DateOfBirth = DateTime.Now
            };

            var user_two = new User
            {
                FirstName = "Test",
                LastName = "Testy",
                EmailAddress = "test@test.com",
                Password = "test",
                DateOfBirth = DateTime.Now
            };

            var workspace_one = new Workspace
            {
                Name = "My Workspace",
                DateAdded = DateTime.Now,
                CreatedByUser = user_two
            };

            var project_one = new Project
            {
                ProjectName = ".NET",
                Workspace = workspace_one
            };

            var tag_one = new Tag
            {
                Name = "Random",
                Description = "Something"
            };

            var session_one = new Session
            {
                Description = "Writing some examples",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                User = user_two,
                Project = project_one
            };

            session_one.Tags.Add(new SessionTag() { Session = session_one, Tag = tag_one });
            using var db = new ProductivityContext();

            db.Users.Add(user_two);
            db.Workspaces.Add(workspace_one);
            db.Projects.Add(project_one);
            db.Sessions.Add(session_one);

            db.SaveChanges();
        }
    }
}
