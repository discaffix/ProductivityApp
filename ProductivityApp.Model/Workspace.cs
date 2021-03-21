using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Model
{
    public class Workspace
    {
        public int WorkspaceId { get; set; }

        public string Name { get; set; }

        public DateTime DateAdded { get; set; }

        public User CreatedByUser { get; set; }

        /// <summary>
        /// A collection of projects (one-to-many)
        /// </summary>
        public ICollection<Project> Projects { get; set; }
    }
}
