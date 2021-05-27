using System.Collections.Generic;

namespace ProductivityApp.Model
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public Workspace Workspace { get; set; }

        public Client Client { get; set; }

        public ICollection<Session> Sessions { get; } = new List<Session>();

    }
}
