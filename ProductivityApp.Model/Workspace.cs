using System;
using System.Collections.Generic;

namespace ProductivityApp.Model
{
    public class Workspace
    {
        public int WorkspaceId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public User CreatedByUser { get; set; }

        /// <summary>
        /// A collection of projects (one-to-many)
        /// </summary>
        public ICollection<Project> Projects { get; set; }
    }
}
