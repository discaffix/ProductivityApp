using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Model
{
    public class Project
    {
        public string ProjectId { get; set; }

        public Workspace Workspace { get; set; }

        public Client Client { get; set; }

        public ICollection<Session> Sessions { get; } = new List<Session>();

    }
}
