using System;
using System.Collections.Generic;

namespace ProductivityApp.Model
{
    public class Client
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        /// <summary>
        /// The property that defines the one-to-many relationship between project and client.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
    }
}
